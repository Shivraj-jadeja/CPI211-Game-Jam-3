using System.Collections;
using UnityEngine;

public class MonsterChaseTrigger : MonoBehaviour
{
    public SimpleMonsterChase monsterChase;
    public float delayBeforeChase = 5f;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("[Trigger] Entered by: " + other.name + " | Tag: " + other.tag);

        if (triggered)
            return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("[Trigger] Player entered chase trigger");
            triggered = true;
            StartCoroutine(StartChaseWithDelay());
        }
        else
        {
            Debug.Log("[Trigger] Not player, ignored");
        }
    }

    IEnumerator StartChaseWithDelay()
    {
        Debug.Log("[Trigger] Waiting " + delayBeforeChase + " seconds before chase");
        yield return new WaitForSeconds(delayBeforeChase);

        if (monsterChase != null)
        {
            Debug.Log("[Trigger] Calling monsterChase.StartChase()");
            monsterChase.StartChase();
        }
        else
        {
            Debug.LogError("[Trigger] monsterChase reference is NULL");
        }
    }
}