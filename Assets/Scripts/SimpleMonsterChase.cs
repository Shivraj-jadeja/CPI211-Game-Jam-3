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
    public float chaseModelYRotation = 20f;
    public float attackModelYRotation = 90f;

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

    private float timer;
    private bool isAttacking = false;
    private Vector3 cameraLocalStartPos;

    public AudioSource stepAudio;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (playerController == null && player != null)
            playerController = player.GetComponent<FPSController>();

        if (playerCamera != null)
            cameraLocalStartPos = playerCamera.localPosition;

        if (monsterVisual != null)
        {
            Vector3 angles = monsterVisual.localEulerAngles;
            angles.y = chaseModelYRotation;
            monsterVisual.localEulerAngles = angles;
        }

        if (agent != null && !agent.isOnNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, sampleRange, NavMesh.AllAreas))
                agent.Warp(hit.position);
        }
    }

    void Update()
    {
        if (!chaseActive || player == null || agent == null || !agent.enabled)
            return;

        if (isAttacking)
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
                agent.SetDestination(hit.position);

            timer = repathRate;
        }
    }

    IEnumerator StartAttack()
    {
        isAttacking = true;
        chaseActive = false;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        if (playerController != null)
            playerController.enabled = false;

        if (monsterVisual != null)
        {
            Vector3 angles = monsterVisual.localEulerAngles;
            angles.y = attackModelYRotation;
            monsterVisual.localEulerAngles = angles;
        }

        if (animator != null)
            animator.SetTrigger("Attack");

        float elapsed = 0f;
        Quaternion startRotation = playerCamera.rotation;

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
        Debug.Log("GAME OVER");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartChase()
    {
        stepAudio.Play();
        if (isAttacking)
            return;

        chaseActive = true;

        if (agent != null)
            agent.isStopped = false;

        if (monsterVisual != null)
        {
            Vector3 angles = monsterVisual.localEulerAngles;
            angles.y = chaseModelYRotation;
            monsterVisual.localEulerAngles = angles;
        }
    }

    public void StopChase()
    {
        stepAudio.Pause();
        chaseActive = false;

        if (agent != null && agent.isOnNavMesh)
            agent.ResetPath();
    }
}