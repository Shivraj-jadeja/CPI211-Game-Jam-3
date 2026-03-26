using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class SyringeInteract : MonoBehaviour
{
    
    public float distance = 0.2f;
    public float duration = 0.2f;
    private Vector3 startLocalPos;
    private bool isMoving = false;
    public float range = 5f;
    public LayerMask Patient;
    public AudioSource AudioSource;
    public VideoPlayer vid;
    public VideoClip dead_oldman;
    public VideoClip bedguy_dead;
    public VideoClip nurse_die;
    public VideoClip girldead;
    private MonoBehaviour FPSController;
    private void Awake()
    {
        startLocalPos = transform.localPosition;
        vid.loopPointReached += OnVideoFinished;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            PerformRaycast();
            StartCoroutine(MoveForwardAndBack());
        }
    }
    IEnumerator MoveForwardAndBack()
    {
        isMoving = true;

        Vector3 forwardPos = startLocalPos + Vector3.forward * distance;

        float halfDuration = duration / 2f;
        float t = 0f;

        // Move forward
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            float lerp = t / halfDuration;
            transform.localPosition = Vector3.Lerp(startLocalPos, forwardPos, lerp);
            yield return null;
        }

        t = 0f;

        // Move back
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            float lerp = t / halfDuration;
            transform.localPosition = Vector3.Lerp(forwardPos, startLocalPos, lerp);
            yield return null;
        }

        transform.localPosition = startLocalPos;
        isMoving = false;
    }

    void PerformRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range, Patient))
        {
            Debug.Log("Hit: " + hit.collider.name + " at distance: " + hit.distance);
            AudioSource.Play();
            if (hit.collider.CompareTag("POld"))
            {
                PlayVideo(dead_oldman);
            }
            if (hit.collider.CompareTag("PBed"))
            {
                PlayVideo(bedguy_dead);
            }
            if (hit.collider.CompareTag("PNurse"))
            {
                PlayVideo(nurse_die);
            }
            if (hit.collider.CompareTag("PWoman"))
            {
                PlayVideo(girldead);
            }
        }
        else
        {
            Debug.Log("aww man");
        }

            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * range, Color.red, 1f);
    }
    void PlayVideo(VideoClip clip)
    {
        if (clip == null) return;

        vid.gameObject.SetActive(true);

        vid.Stop();
        vid.clip = clip;
        FPSController = GetComponent<MonoBehaviour>();
        FPSController.enabled = false;
        vid.Play();
    }
    void OnVideoFinished(VideoPlayer vp)
    {
        vid.Stop();

        // Hide the video (depends how you're displaying it)
        vid.gameObject.SetActive(false);

        // Re-enable player control
        if (FPSController != null)
            FPSController.enabled = true;
    }
}
