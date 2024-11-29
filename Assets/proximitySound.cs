using UnityEngine;
using System.Collections; // Add this to resolve the IEnumerator issue

[RequireComponent(typeof(AudioSource))]
public class TriggerMusicController : MonoBehaviour
{
    public float fadeDuration = 1f; // Duration for fade-in and fade-out
    public AudioSource audioSource; // Reference to the AudioSource component
    private Coroutine fadeCoroutine; // Coroutine reference for fading

    void Awake()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Ensure the AudioSource starts silent
        if (audioSource != null)
        {
            audioSource.volume = 0f;
            audioSource.loop = true; // Optional: make the music loop
            audioSource.playOnAwake = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure only the player triggers the music
        {
            // Start or fade in the music
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeInMusic());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure only the player triggers the music
        {
            // Fade out the music
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeOutMusic());
        }
    }

    // Coroutine to fade in the music
    private IEnumerator FadeInMusic()
    {
        if (!audioSource.isPlaying) audioSource.Play(); // Start playing if not already

        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 1f; // Ensure the volume is at max
    }

    // Coroutine to fade out the music
    private IEnumerator FadeOutMusic()
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0f; // Ensure the volume is completely off
        audioSource.Stop(); // Stop playing the music
    }
}
