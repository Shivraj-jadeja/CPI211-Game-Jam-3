using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public DialogueDatabase database;

    public void ShowLine(string text, float duration = 4f)
    {
        if (dialogueText == null)
        {
            Debug.LogWarning("Dialogue text is not assigned.");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(ShowLineRoutine(text, duration));
    }

    public void ShowDialogue(string id, float duration = 4f)
    {
        if (database == null)
        {
            Debug.LogWarning("DialogueDatabase is not assigned.");
            return;
        }

        if (!database.lines.ContainsKey(id))
        {
            Debug.LogWarning("Dialogue ID not found: " + id);
            return;
        }

        ShowLine(database.lines[id], duration);
    }

    private IEnumerator ShowLineRoutine(string text, float duration)
    {
        dialogueText.text = text;
        yield return new WaitForSeconds(duration);
        dialogueText.text = "";
    }
}