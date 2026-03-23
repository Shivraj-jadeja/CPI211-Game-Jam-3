using UnityEngine;

public class GrabItem : MonoBehaviour
{
    public float grabRange = 3f;
    public Transform holdPoint;

    GameObject heldItem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (heldItem == null)
            {
                TryGrab();
            }
            else
            {
                DropItem();
            }
        }
    }

    void TryGrab()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, grabRange))
        {
            if (hit.collider.CompareTag("Grabbable"))
            {
                heldItem = hit.collider.gameObject;
                heldItem.GetComponent<Rigidbody>().isKinematic = true;
                heldItem.transform.position = holdPoint.position;
                heldItem.transform.parent = holdPoint;
            }
        }
    }

    void DropItem()
    {
        heldItem.GetComponent<Rigidbody>().isKinematic = false;
        heldItem.transform.parent = null;
        heldItem = null;
    }
}