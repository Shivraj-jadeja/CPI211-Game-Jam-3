using UnityEngine;
using System.Collections;

public class FinalDialogue : MonoBehaviour
{
    public DialogueManager dialogueManager;

    public string firstID = "FINAL_01";
    public string secondID = "FINAL_02";

    public float delayBetweenLines = 4f;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (!other.transform.root.CompareTag("Player")) return;

        hasTriggered = true;
        StartCoroutine(PlayDialogueSequence());
    }

    private IEnumerator PlayDialogueSequence()
    {
        if (dialogueManager == null)
        {
            Debug.LogWarning("DialogueManager not assigned.");
            yield break;
        }

        // First line
        dialogueManager.ShowDialogue(firstID, delayBetweenLines);

        yield return new WaitForSeconds(delayBetweenLines);

        // Second line
        dialogueManager.ShowDialogue(secondID, delayBetweenLines);
    }
}