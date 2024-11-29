// using UnityEngine;

// public class Player : MonoBehaviour
// {
//     public static Player Instance;

//     private void Awake()
//     {
//         // Singleton pattern
//         if (Instance == null)
//         {
//             Instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     // Teleports the player to the respawn point
//     public void RespawnToStart()
//     {
//         if (GameManager.Instance.respawnPoint != null)
//         {
//             // Move the player's entire hierarchy to the respawn point
//             transform.position = GameManager.Instance.respawnPoint.position;
//             transform.rotation = GameManager.Instance.respawnPoint.rotation;
//         }
//         else
//         {
//             Debug.LogWarning("Respawn Point is not set in the GameManager!");
//         }
//     }
// }



using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    private AudioSource audioSource; // Reference to the AudioSource for walking sound
    private CharacterController characterController; // Reference to the CharacterController

    public AudioClip walkingSound; // Walking sound clip
    public float walkSoundThreshold = 0.1f; // Minimum movement required to play sound

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Get components
        audioSource = gameObject.AddComponent<AudioSource>(); // Add an AudioSource if not already present
        characterController = GetComponent<CharacterController>();

        // Configure the AudioSource
        if (walkingSound != null)
        {
            audioSource.clip = walkingSound;
            audioSource.loop = true; // Set the sound to loop
            audioSource.playOnAwake = false; // Do not play automatically
        }
    }

    private void Update()
    {
        CheckMovementAndPlaySound();
    }

    public void RespawnToStart()
    {
        // Specify the hardcoded position and rotation values
        Vector3 respawnPosition = new Vector3(-22, 0.4f, 7); // Replace with your desired position
        Quaternion respawnRotation = Quaternion.Euler(0, 0, 0); // Replace with your desired rotation

        // Apply the hardcoded position and rotation
        transform.position = respawnPosition;
        transform.rotation = respawnRotation;

        Debug.Log("Player teleported to hardcoded position: " + transform.position);
    }



    private void CheckMovementAndPlaySound()
    {
        // Determine if the player is moving
        bool isMoving = false;

        if (characterController != null)
        {
            // Use CharacterController's velocity to detect movement
            isMoving = characterController.velocity.magnitude > walkSoundThreshold;
        }
        else
        {
            // Fallback to input axes for basic movement detection
            isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
        }

        // Play or stop walking sound based on movement
        if (isMoving && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (!isMoving && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
