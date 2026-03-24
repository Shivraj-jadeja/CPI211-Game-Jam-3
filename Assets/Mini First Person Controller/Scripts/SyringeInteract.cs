using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class SyringeInteract : MonoBehaviour
{
    
    public float distance = 0.2f;
    public float duration = 0.2f;
    private Vector3 startLocalPos;
    private bool isMoving = false;
    public float range = 5f;
    public LayerMask Patient;
    private void Awake()
    {
        startLocalPos = transform.localPosition;
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
        }
        else
        {
            Debug.Log("aww man");
        }

            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * range, Color.red, 1f);
    }
}
