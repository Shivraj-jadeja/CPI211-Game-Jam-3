using UnityEngine;
using System.Collections;

public class FirstMoveDialogue : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public DialogueDatabase dialogueDatabase;

    void Update()
    {
        if (!GameManager.Instance.introFinished) return;
        if (GameManager.Instance.firstMoveDialoguePlayed) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0)
        {
            GameManager.Instance.firstMoveDialoguePlayed = true;
            StartCoroutine(PlayFirstMoveLines());
        }
    }

    IEnumerator PlayFirstMoveLines()
    {
        dialogueManager.ShowLine(dialogueDatabase.lines["S1_Boss_03"], 4f);
        yield return new WaitForSeconds(4.5f);

        dialogueManager.ShowLine(dialogueDatabase.lines["S1_Boss_04"], 5f);
    }
}