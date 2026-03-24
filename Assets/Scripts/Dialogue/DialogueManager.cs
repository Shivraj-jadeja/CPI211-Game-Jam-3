using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    public void ShowLine(string text, float duration = 4f)
    {
        StopAllCoroutines();
        StartCoroutine(ShowLineRoutine(text, duration));
    }

    IEnumerator ShowLineRoutine(string text, float duration)
    {
        dialogueText.text = text;
        yield return new WaitForSeconds(duration);
        dialogueText.text = "";
    }
}