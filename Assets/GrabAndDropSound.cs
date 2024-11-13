using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabSound : MonoBehaviour
{
    public AudioClip grabSound;
    public AudioClip dropSound;

    private AudioSource audioSource;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Subscribe to the grab and drop events
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnDrop);
    }

    void OnGrab(XRBaseInteractor interactor)
    {
        PlaySound(grabSound);
    }

    void OnDrop(XRBaseInteractor interactor)
    {
        PlaySound(dropSound);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the events when the object is destroyed
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnDrop);
    }
}
