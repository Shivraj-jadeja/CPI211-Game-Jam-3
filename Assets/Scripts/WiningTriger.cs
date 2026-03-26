using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.transform.root.CompareTag("Player")) return;

        hasTriggered = true;

        if (EndingVideoManager.Instance != null)
        {
            EndingVideoManager.Instance.PlayEndingVideo();
        }
        else
        {
            Debug.LogWarning("EndingVideoManager.Instance is missing.");
        }
    }
}