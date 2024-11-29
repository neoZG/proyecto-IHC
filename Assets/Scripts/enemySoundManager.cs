using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    private AudioSource audioSource; // Reference to the AudioSource for the eerie sound
    private bool isSoundPlaying = false; // Flag to check if the sound is playing

    // SoundManager Singleton setup
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
            }
            return instance;
        }
    }

    void Awake()
    {
        // Ensure there is only one SoundManager
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicates
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the SoundManager across scenes
        }

        // Create or get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.volume = 0.3f; // Set a default volume
        audioSource.loop = true; // Loop the sound
    }

    // Play the eerie sound when chasing starts
    public void PlayEerieSound(AudioClip clip)
    {
        if (!isSoundPlaying)
        {
            audioSource.clip = clip; // Set the eerie sound clip
            audioSource.Play(); // Start playing the sound
            isSoundPlaying = true;
        }
    }

    // Stop the eerie sound when the player is lost or no enemies are chasing
    public void StopEerieSound()
    {
        if (isSoundPlaying)
        {
            audioSource.Stop(); // Stop the sound
            isSoundPlaying = false;
        }
    }
}
