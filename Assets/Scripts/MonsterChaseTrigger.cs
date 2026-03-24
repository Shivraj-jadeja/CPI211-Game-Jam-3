using System.Collections;
using UnityEngine;

public class MonsterChaseTrigger : MonoBehaviour
{
    public SimpleMonsterChase monsterChase;
    public float delayBeforeChase = 5f;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(StartChaseWithDelay());
        }
    }

    IEnumerator StartChaseWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeChase);

        if (monsterChase != null)
            monsterChase.StartChase();
    }
}