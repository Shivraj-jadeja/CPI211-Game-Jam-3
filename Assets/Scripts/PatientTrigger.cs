using UnityEngine;
using UnityEngine.Video;

public class PatientTrigger : MonoBehaviour
{
    [SerializeField] private int patientNum;
    private VideoPlayer video;
    private MonoBehaviour FPSController;
    private void Start()
    {
        video = GetComponent<VideoPlayer>();
        video.loopPointReached += OnVideoEnd;
    }


    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            FPSController = other.GetComponent<MonoBehaviour>();
            FPSController.enabled = false;
            video.Play();
        }
    }

    [System.Obsolete]
    private void OnVideoEnd(VideoPlayer vp)
    {
        // Re-enable movement after video finishes
        if (FPSController != null)
        {
            FPSController.enabled = true;
        }
        vp.enabled = false;
        Destroy(gameObject);
    }
}
