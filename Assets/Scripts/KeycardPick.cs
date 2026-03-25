using UnityEngine;

public class KeycardPick : MonoBehaviour
{
    public GameObject promptUI;
    public Transform player;
    public Transform holdPoint;
    public float interactDistance = 3f;
    public KeyCode pickupKey = KeyCode.F;

    private bool isHeld = false;
    private Rigidbody rb;
    private Collider col;

    void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    void Update()
    {
        if (player == null || promptUI == null || holdPoint == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (!isHeld)
        {
            if (distance < interactDistance)
            {
                promptUI.SetActive(true);

                if (Input.GetKeyDown(pickupKey))
                {
                    GrabKeycard();
                }
            }
            else
            {
                promptUI.SetActive(false);
            }
        }
        else
        {
            if (Input.GetKeyDown(pickupKey))
            {
                DropKeycard();
            }
        }
    }

    void GrabKeycard()
    {
        isHeld = true;
        promptUI.SetActive(false);

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (col != null)
        {
            col.enabled = false;
        }

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    void DropKeycard()
    {
        isHeld = false;
        transform.SetParent(null);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        if (col != null)
        {
            col.enabled = true;
        }
    }

    public bool IsHeld()
    {
        return isHeld;
    }
}