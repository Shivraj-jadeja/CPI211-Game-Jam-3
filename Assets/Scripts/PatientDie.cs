using UnityEngine;

public class PatientDie : MonoBehaviour
{
    [SerializeField]public GameObject newFBXPrefab;
    void ReplaceObject()
    {
        Vector3 oldPos = transform.position;
        Quaternion oldRot = transform.rotation;
        Destroy(gameObject);
        Instantiate(newFBXPrefab, oldPos, oldRot);
    }
}