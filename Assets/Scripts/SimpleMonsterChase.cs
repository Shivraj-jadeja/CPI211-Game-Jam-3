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

    [Header("Chase")]
    public bool chaseActive = false;
    public float repathRate = 0.1f;
    public float sampleRange = 0.75f;
    public float attackDistance = 1.5f;

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

        Debug.Log("[Monster] Start complete | player=" + (player != null) +
                  " | agent=" + (agent != null) +
                  " | animator=" + (animator != null));
    }

    void Update()
    {
        if (!chaseActive)
            return;

        if (player == null)
        {
            Debug.LogError("[Monster] Player reference is NULL");
            return;
        }

        if (agent == null)
        {
            Debug.LogError("[Monster] Agent reference is NULL");
            return;
        }

        if (!agent.enabled)
        {
            Debug.LogWarning("[Monster] Agent is disabled");
            return;
        }

        if (isAttacking)
            return;

        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning("[Monster] Agent is not on NavMesh during chase");
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackDistance)
        {
            Debug.Log("[Monster] Player in attack distance");
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
                Debug.Log("[Monster] SetDestination -> " + hit.position);
            }
            else
            {
                Debug.LogWarning("[Monster] Could not sample player position on NavMesh");
            }

            timer = repathRate;
        }
    }

    IEnumerator StartAttack()
    {
        Debug.Log("[Monster] StartAttack called");

        isAttacking = true;
        chaseActive = false;

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
                Vector3 targetPos;

                if (monsterLookTarget != null)
                    targetPos = monsterLookTarget.position;
                else
                    targetPos = transform.position + Vector3.up * 1.4f;

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
        Debug.Log("[Monster] GAME OVER");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartChase()
    {
        Debug.Log("[Monster] StartChase called");

        if (isAttacking)
        {
            Debug.LogWarning("[Monster] Cannot start chase, already attacking");
            return;
        }

        chaseActive = true;

        if (stepAudio != null)
            stepAudio.Play();
        else
            Debug.LogWarning("[Monster] Step Audio is not assigned");

        if (agent != null)
        {
            agent.isStopped = false;
            Debug.Log("[Monster] Agent enabled=" + agent.enabled + " | onNavMesh=" + agent.isOnNavMesh);
        }
        else
        {
            Debug.LogError("[Monster] Agent is NULL in StartChase");
        }

        SetModelYRotation(chaseModelYRotation);

        if (animator != null)
            animator.speed = 1f;
    }

    public void StopChase()
    {
        Debug.Log("[Monster] StopChase called");

        if (stepAudio != null)
            stepAudio.Pause();

        chaseActive = false;

        if (agent != null && agent.isOnNavMesh)
            agent.ResetPath();

        SetModelYRotation(idleModelYRotation);

        if (freezeAnimatorUntilChase && animator != null && !isAttacking)
            animator.speed = 0f;
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