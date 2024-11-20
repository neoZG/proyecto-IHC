using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Collectible : MonoBehaviour
{
    private AudioSource audioSource;
    private XRGrabInteractable grabInteractable;

    private void Start()
    {
        // Configura el AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("El objeto no tiene un AudioSource. Por favor, agrégalo al GameObject.");
        }

        // Configura el XRGrabInteractable
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab); // Escucha cuando el objeto sea agarrado
        }
        else
        {
            Debug.LogError("El objeto no tiene un XRGrabInteractable. Por favor, agrégalo al GameObject.");
        }
    }

    private void Update()
    {
        // Rotación para hacerlo más atractivo visualmente
        transform.localRotation = Quaternion.Euler(270f, Time.time * 100f, 0f);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Reproduce el sonido
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Desactiva el objeto después de un pequeño retraso para que el sonido se escuche
        Invoke(nameof(DisableObject), 0.5f);
    }

    private void DisableObject()
    {
        gameObject.SetActive(false); // Desactiva el objeto
    }
}
