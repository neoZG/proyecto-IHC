using UnityEngine;

public class PlaySoundOnTrigger : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Cambia "Player" por el tag de tu personaje
        {
            audioSource.Play();
        }
    }
}
