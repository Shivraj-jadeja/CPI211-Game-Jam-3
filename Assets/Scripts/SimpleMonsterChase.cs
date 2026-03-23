using UnityEngine;
using UnityEngine.AI;

public class SimpleMonsterChase : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public bool chaseActive = false;
    public float repathRate = 0.2f;
    public float sampleRange = 0.05f;

    private float timer;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (agent != null && !agent.isOnNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, sampleRange, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }
        }
    }

    void Update()
    {
        if (!chaseActive || player == null || agent == null || !agent.enabled)
            return;

        if (!agent.isOnNavMesh)
            return;

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
    }

    public void StartChase()
    {
        chaseActive = true;
    }

    public void StopChase()
    {
        chaseActive = false;

        if (agent != null && agent.isOnNavMesh)
            agent.ResetPath();
    }
}