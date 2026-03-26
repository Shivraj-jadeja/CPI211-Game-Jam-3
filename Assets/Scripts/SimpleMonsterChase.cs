using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SimpleMonsterChase : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform playerCamera;
    public FPSController playerController;
    public NavMeshAgent agent;
    public Animator animator;
    public Transform monsterLookTarget;
    public Transform monsterVisual;

    [Header("Visual Rotation")]
    public float idleModelYRotation = 90f;
    public float chaseModelYRotation = 20f;
    public float attackModelYRotation = 90f;

    [Header("Animation")]
    public bool freezeAnimatorUntilChase = true;
    public float screamDuration = 2f;
    public float screamAnimationSpeed = 0.75f;
    public float screamFacingYRotation = 0f;

    [Header("Chase")]
    public bool chaseActive = false;
    public float repathRate = 0.1f;
    public float sampleRange = 0.75f;
    public float attackDistance = 1.5f;
    public float chaseTurnSpeed = 8f;

    [Header("Attack")]
    public float lookAtDuration = 0.25f;
    public float gameOverDelay = 2.5f;

    [Header("Camera Shake")]
    public float shakeAmount = 0.04f;
    public float shakeSpeed = 25f;

    [Header("Audio")]
    public AudioSource stepAudio;

    private float timer;
    private bool isAttacking = false;
    private bool isScreaming = false;
    private Vector3 cameraLocalStartPos;

    void Start()
    {
        Debug.Log("[Monster] Start called");

        chaseActive = false;

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (playerController == null && player != null)
            playerController = player.GetComponent<FPSController>();

        if (playerCamera != null)
            cameraLocalStartPos = playerCamera.localPosition;

        if (agent != null)
            agent.updateRotation = false;

        SetModelYRotation(idleModelYRotation);

        if (freezeAnimatorUntilChase && animator != null)
            animator.speed = 0f;

        if (agent == null)
        {
            Debug.LogError("[Monster] NavMeshAgent is NULL");
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning("[Monster] Agent not on NavMesh at start, trying warp");

            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, sampleRange, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
                Debug.Log("[Monster] Warped agent to NavMesh");
            }
            else
            {
                Debug.LogError("[Monster] Could not find nearby NavMesh");
            }
        }

        Debug.Log("[Monster] Start complete");
    }

    void Update()
    {
        if (!chaseActive)
            return;

        if (player == null || agent == null || !agent.enabled)
            return;

        if (isAttacking || isScreaming)
            return;

        if (!agent.isOnNavMesh)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackDistance)
        {
            StartCoroutine(StartAttack());
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(player.position, out hit, sampleRange, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }

            timer = repathRate;
        }

        RotateTowardsMovement();
    }

    public void StartScreamThenChase()
    {
        Debug.Log("[Monster] StartScreamThenChase() called");

        if (isAttacking || isScreaming || chaseActive)
        {
            Debug.LogWarning("[Monster] Cannot start scream/chase right now");
            return;
        }

        StartCoroutine(ScreamThenChaseRoutine());
    }

    IEnumerator ScreamThenChaseRoutine()
    {
        Debug.Log("[Monster] Scream routine started");
        isScreaming = true;
        chaseActive = false;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        FacePlayerOnce();
        SetModelYRotation(screamFacingYRotation);

        if (animator != null)
        {
            animator.speed = screamAnimationSpeed;
            animator.ResetTrigger("Attack");
            animator.ResetTrigger("Scream");
            animator.SetTrigger("Scream");
            Debug.Log("[Monster] Scream trigger set");
        }
        else
        {
            Debug.LogWarning("[Monster] Animator missing");
        }

        yield return new WaitForSeconds(screamDuration / screamAnimationSpeed);

        Debug.Log("[Monster] Scream finished -> StartChase()");
        isScreaming = false;
        StartChase();
    }

    IEnumerator StartAttack()
    {
        isAttacking = true;
        chaseActive = false;
        isScreaming = false;

        if (stepAudio != null)
            stepAudio.Pause();

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        if (playerController != null)
            playerController.enabled = false;

        SetModelYRotation(attackModelYRotation);

        if (animator != null)
        {
            animator.speed = 1f;
            animator.SetTrigger("Attack");
        }

        float elapsed = 0f;
        Quaternion startRotation = playerCamera != null ? playerCamera.rotation : Quaternion.identity;

        while (elapsed < gameOverDelay)
        {
            if (playerCamera != null)
            {
                Vector3 targetPos = monsterLookTarget != null
                    ? monsterLookTarget.position
                    : transform.position + Vector3.up * 1.4f;

                Vector3 direction = targetPos - playerCamera.position;

                if (direction.sqrMagnitude > 0.001f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);

                    if (elapsed < lookAtDuration)
                    {
                        float t = elapsed / lookAtDuration;
                        playerCamera.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                    }
                    else
                    {
                        playerCamera.rotation = targetRotation;
                    }
                }

                Vector3 shakeOffset = Vector3.zero;
                shakeOffset.x = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * 2f * shakeAmount;
                shakeOffset.y = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * 2f * shakeAmount;

                playerCamera.localPosition = cameraLocalStartPos + shakeOffset;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (playerCamera != null)
            playerCamera.localPosition = cameraLocalStartPos;

        GameOver();
    }

    void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartChase()
    {
        Debug.Log("[Monster] StartChase() called");

        if (isAttacking)
            return;

        chaseActive = true;

        if (stepAudio != null)
            stepAudio.Play();

        if (agent != null)
            agent.isStopped = false;

        if (animator != null)
            animator.speed = 1f;
    }

    public void StopChase()
    {
        if (stepAudio != null)
            stepAudio.Pause();

        chaseActive = false;
        isScreaming = false;

        if (agent != null && agent.isOnNavMesh)
            agent.ResetPath();

        SetModelYRotation(idleModelYRotation);

        if (freezeAnimatorUntilChase && animator != null && !isAttacking)
            animator.speed = 0f;
    }

    void FacePlayerOnce()
    {
        if (player == null)
            return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        transform.rotation = Quaternion.LookRotation(direction.normalized);
    }

    void RotateTowardsMovement()
    {
        if (agent == null)
            return;

        Vector3 direction = agent.desiredVelocity;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            chaseTurnSpeed * Time.deltaTime
        );
    }

    void SetModelYRotation(float yRotation)
    {
        if (monsterVisual == null)
            return;

        Vector3 angles = monsterVisual.localEulerAngles;
        angles.y = yRotation;
        monsterVisual.localEulerAngles = angles;
    }
}