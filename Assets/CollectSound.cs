using UnityEngine;

public class CollectSound : MonoBehaviour
{
    public AudioClip collectSound;  // The sound to play when collected
    private AudioSource audioSource;
    

    void Awake()
    {
        // Add the AudioSource component to play the sound
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider that triggered the event is the player
        if (other.CompareTag("Player"))
        {
            // Play the collect sound if the AudioClip is assigned
            if (collectSound != null)
            {
                audioSource.PlayOneShot(collectSound);
            }

            // Call any other logic for collecting the item here
            // For example: ObjectCollectible.OnCollected?.Invoke(); if you're tracking the collection
            Destroy(gameObject);  // Destroy the object after it's collected
        }
    }
}
