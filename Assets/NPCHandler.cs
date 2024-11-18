using System.Collections;
using UnityEngine;

public class NPCEventTrigger : MonoBehaviour
{
    public Animator npcAnimator;            // Reference to the NPC Animator
    public AudioSource voiceOverSource;     // Reference to the AudioSource for the voice-over
    public GameObject hiddenObject;         // Reference to the hidden object
    public float objectVisibleDuration = 5f; // Duration the object and animation remain visible
    public GameObject triggerObject;        // Reference to the trigger object itself
    public DialogueBox dialogueBox;         // Reference to the DialogueBox script for subtitles

    private void Start()
    {
        // Disable the animator at the start
        if (npcAnimator != null)
        {
            npcAnimator.enabled = false;
        }

        // Hide the hidden object initially
        if (hiddenObject != null)
        {
            hiddenObject.SetActive(false);
        }

        // Hide the subtitle initially
        if (dialogueBox != null)
        {
            dialogueBox.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player")) // Assumes your player has the "Player" tag
        {
            // Start NPC animation for a specific duration
            if (npcAnimator != null)
            {
                npcAnimator.enabled = true;
                npcAnimator.SetTrigger("Start"); // Trigger the animation
                StartCoroutine(StopAnimatorAfterDuration());
            }

            // Play voice-over
            if (voiceOverSource != null && !voiceOverSource.isPlaying)
            {
                voiceOverSource.Play();
            }

            // Show hidden object for a specific duration
            if (hiddenObject != null)
            {
                StartCoroutine(ShowObjectTemporarily());
            }

            // Enable the subtitle system when the trigger happens
            if (dialogueBox != null)
            {
                dialogueBox.StartDialogue();  // Start the dialogue only when the trigger is activated
            }

            // Disable the trigger to ensure it activates only once
            if (triggerObject != null)
            {
                triggerObject.SetActive(false);
            }
        }
    }

    private IEnumerator ShowObjectTemporarily()
    {
        // Make the object visible
        hiddenObject.SetActive(true);

        // Wait for the specified duration
        yield return new WaitForSeconds(objectVisibleDuration);

        // Hide the object again
        hiddenObject.SetActive(false);
    }

    private IEnumerator StopAnimatorAfterDuration()
    {
        // Wait for the same duration as the visible object
        yield return new WaitForSeconds(objectVisibleDuration);

        // Set the animator back to an idle state or stop the animation explicitly
        if (npcAnimator != null)
        {
            // Set to idle state (assuming you have an "Idle" animation or state)
            npcAnimator.SetTrigger("Idle");  // Replace "Idle" with your idle animation trigger name
        }
    }
}
