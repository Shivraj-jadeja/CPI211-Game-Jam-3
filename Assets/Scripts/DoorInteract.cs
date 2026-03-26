using System.Collections;
using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    [Header("Interaction")]
    public Transform player;
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Door Rotation")]
    public Transform doorPivot;
    public float openAngle = 90f;
    public float openSpeed = 2f;

    [Header("UI")]
    public GameObject interactPromptUI;

    [Header("Audio")]
    public AudioSource doorAudio;

    private bool isOpen = false;
    private bool isMoving = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        if (doorPivot == null)
            doorPivot = transform;

        closedRotation = doorPivot.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);

        if (interactPromptUI != null)
            interactPromptUI.SetActive(false);
    }

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(player.position, doorPivot.position);

        if (distance <= interactDistance)
        {
            if (interactPromptUI != null)
                interactPromptUI.SetActive(true);

            if (Input.GetKeyDown(interactKey) && !isMoving)
            {
                if (doorAudio != null)
                    doorAudio.Play();

                StartCoroutine(RotateDoor(isOpen ? closedRotation : openRotation));
                isOpen = !isOpen;
            }
        }
        else
        {
            if (interactPromptUI != null)
                interactPromptUI.SetActive(false);
        }
    }

    IEnumerator RotateDoor(Quaternion targetRotation)
    {
        isMoving = true;

        Quaternion startRotation = doorPivot.localRotation;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            doorPivot.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        doorPivot.localRotation = targetRotation;
        isMoving = false;
    }
}