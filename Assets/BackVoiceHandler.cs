// using UnityEngine;

// public class VoiceTrigger : MonoBehaviour
// {
//     public AudioSource voiceOverSource;     // Reference to the AudioSource for the background voice
//     public GameObject triggerObject;        // Reference to the trigger object itself

//     private void Start()
//     {
//         // Ensure the AudioSource is properly assigned
//         if (voiceOverSource == null)
//         {
//             Debug.LogError("AudioSource is not assigned.");
//         }
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         // Check if the player entered the trigger
//         if (other.CompareTag("Player")) // Assumes your player has the "Player" tag
//         {
//             // Play voice-over if it's not already playing
//             if (voiceOverSource != null && !voiceOverSource.isPlaying)
//             {
//                 voiceOverSource.Play();
//             }

//             // Disable the trigger to ensure it activates only once
//             if (triggerObject != null)
//             {
//                 triggerObject.SetActive(false);
//             }
//         }
//     }
// }

using UnityEngine;

public class VoiceAndCanvasTrigger : MonoBehaviour
{
    public AudioSource voiceOverSource;     // Reference to the AudioSource for the background voice
    public GameObject triggerObject;        // Reference to the trigger object itself
    public Canvas instructionCanvas;        // Reference to the Canvas to display instructions
    public float canvasVisibleDuration = 5f; // Duration to display the Canvas (optional)

    private Collider _triggerCollider;      // Reference to the collider component of the trigger

    private void Start()
    {
        // Ensure the AudioSource is properly assigned
        if (voiceOverSource == null)
        {
            Debug.LogError("AudioSource is not assigned.");
        }

        // Get and disable the trigger collider initially
        _triggerCollider = triggerObject.GetComponent<Collider>();
        if (_triggerCollider == null)
        {
            Debug.LogError("TriggerObject does not have a Collider component.");
        }

        // Hide the Canvas initially
        if (instructionCanvas != null)
        {
            instructionCanvas.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player")) // Assumes your player has the "Player" tag
        {
            // Play voice-over if it's not already playing
            if (voiceOverSource != null && !voiceOverSource.isPlaying)
            {
                voiceOverSource.Play();
            }

            // Show the instruction Canvas
            if (instructionCanvas != null)
            {
                instructionCanvas.enabled = true;

                // Optionally hide the Canvas after a set duration
                StartCoroutine(HideCanvasAfterDuration());
            }

            // Disable the collider to prevent re-triggering
            if (_triggerCollider != null)
            {
                _triggerCollider.enabled = false;
            }
        }
    }

    private System.Collections.IEnumerator HideCanvasAfterDuration()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(canvasVisibleDuration);

        // Hide the Canvas
        if (instructionCanvas != null)
        {
            instructionCanvas.enabled = false;
        }

        // Optionally, deactivate the trigger GameObject at this point
        if (triggerObject != null)
        {
            triggerObject.SetActive(false);
        }
    }
}
