using System.Collections;
using TMPro;
using UnityEngine;

public class FinalDoorKeycard : MonoBehaviour
{
    [Header("Player Setup")]
    public Camera playerCamera;
    public Transform holdPoint;
    public Transform keycard;
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Door Setup")]
    public Transform doorPivot;
    public float openAngle = 90f;
    public float openSpeed = 2f;

    [Header("UI")]
    public TextMeshProUGUI promptText;
    public float messageTime = 2f;

    private bool isOpen = false;
    private bool isAnimating = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine messageRoutine;

    void Start()
    {
        if (doorPivot == null)
            doorPivot = transform;

        closedRotation = doorPivot.rotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);

        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isOpen || isAnimating)
            return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                if (Input.GetKeyDown(interactKey))
                {
                    if (PlayerIsHoldingKeycard())
                    {
                        StartCoroutine(OpenDoor());
                    }
                    else
                    {
                        ShowMessage("Requires Keycard to Exit!");
                    }
                }
            }
        }
    }

    bool PlayerIsHoldingKeycard()
    {
        if (keycard == null || holdPoint == null)
            return false;

        return keycard.IsChildOf(holdPoint);
    }

    IEnumerator OpenDoor()
    {
        isAnimating = true;

        while (Quaternion.Angle(doorPivot.rotation, openRotation) > 0.5f)
        {
            doorPivot.rotation = Quaternion.Slerp(doorPivot.rotation, openRotation, Time.deltaTime * openSpeed);
            yield return null;
        }

        doorPivot.rotation = openRotation;
        isOpen = true;
        isAnimating = false;
    }

    void ShowMessage(string message)
    {
        if (promptText == null)
            return;

        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine = StartCoroutine(ShowMessageRoutine(message));
    }

    IEnumerator ShowMessageRoutine(string message)
    {
        promptText.text = message;
        promptText.gameObject.SetActive(true);

        yield return new WaitForSeconds(messageTime);

        promptText.gameObject.SetActive(false);
    }
}