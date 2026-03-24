using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroSequence : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public DialogueDatabase dialogueDatabase;
    public Image fadePanel;

    public float fadeDuration = 2f;

    IEnumerator Start()
    {
        GameManager.Instance.canPlayerMove = false;

        // Start fully black
        SetFadeAlpha(1f);

        // Small pause
        yield return new WaitForSeconds(0.5f);

        // First boss line
        dialogueManager.ShowLine(dialogueDatabase.lines["S1_Boss_01"], 4f);
        yield return new WaitForSeconds(4.5f);

        // Second boss line + fade in at same time
        dialogueManager.ShowLine(dialogueDatabase.lines["S1_Boss_02"], 5f);
        yield return StartCoroutine(FadeFromBlack());

        yield return new WaitForSeconds(3f);

        GameManager.Instance.canPlayerMove = true;
        GameManager.Instance.introFinished = true;
    }

    IEnumerator FadeFromBlack()
    {
        float time = 0f;
        Color c = fadePanel.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            fadePanel.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        fadePanel.color = new Color(c.r, c.g, c.b, 0f);
    }

    void SetFadeAlpha(float alpha)
    {
        Color c = fadePanel.color;
        fadePanel.color = new Color(c.r, c.g, c.b, alpha);
    }
}