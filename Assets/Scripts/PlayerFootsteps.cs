using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerFootsteps : MonoBehaviour
{
    public FPSController playerController;
    public CharacterController characterController;
    public AudioSource footstepAudio;

    public float minMoveAmount = 0.1f;

    void Start()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        if (footstepAudio == null)
            footstepAudio = GetComponent<AudioSource>();

        if (playerController == null)
            playerController = GetComponent<FPSController>();

        if (footstepAudio != null)
        {
            footstepAudio.loop = true;
            footstepAudio.playOnAwake = false;
            footstepAudio.Stop();
        }
    }

    void Update()
    {
        if (footstepAudio == null || characterController == null)
            return;

        bool controllerEnabled = playerController == null || playerController.enabled;
        bool isMoving = characterController.velocity.magnitude > minMoveAmount;
        bool groundedOrCloseEnough = characterController.isGrounded || Mathf.Abs(characterController.velocity.y) < 0.2f;

        if (controllerEnabled && isMoving && groundedOrCloseEnough)
        {
            if (!footstepAudio.isPlaying)
                footstepAudio.Play();
        }
        else
        {
            if (footstepAudio.isPlaying)
                footstepAudio.Stop();
        }
    }
}