using UnityEngine;

public class PickupLook : MonoBehaviour
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
                    GrabItem();
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
                DropItem();
            }
        }
    }

    void GrabItem()
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

    // Right-hand style position
    transform.localPosition = Vector3.zero;

    // Angled so player sees the whole object better
    transform.localRotation = Quaternion.Euler(-11.886f, 62.13f, -20.21f);
}

    void DropItem()
    {
        isHeld = false;

        // unparent
        transform.SetParent(null);

        // re-enable physics
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
}