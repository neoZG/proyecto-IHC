using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;


public class SocketResizeHandler : MonoBehaviour
{
    public Vector3 targetScale = Vector3.one; // The scale the object should have when in the socket
    private Vector3 originalScale;           // Store the original scale of the object
    private Transform currentObject;         // The interactable currently in the socket

    private XRSocketInteractor socketInteractor;

    private void Awake()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        if (socketInteractor == null)
        {
            Debug.LogError("SocketResizeHandler requires an XRSocketInteractor component.");
        }

        // Subscribe to events
        socketInteractor.selectEntered.AddListener(OnSelectEntered);
        socketInteractor.selectExited.AddListener(OnSelectExited);
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnSelectEntered);
            socketInteractor.selectExited.RemoveListener(OnSelectExited);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        currentObject = args.interactableObject.transform;
        originalScale = currentObject.localScale;
        StartCoroutine(ResizeObject(currentObject, targetScale, 0.5f)); // Smooth resize over 0.5 seconds
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (currentObject != null)
        {
            StartCoroutine(ResizeObject(currentObject, originalScale, 0.5f)); // Smooth revert over 0.5 seconds
            currentObject = null;
        }
    }

    private IEnumerator ResizeObject(Transform obj, Vector3 targetScale, float duration)
    {
        Vector3 initialScale = obj.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.localScale = targetScale;
    }

    // private void OnSelectEntered(SelectEnterEventArgs args)
    // {
    //     // Get the interactable object
    //     currentObject = args.interactableObject.transform;

    //     // Save its original scale
    //     originalScale = currentObject.localScale;

    //     // Resize the object to fit in the socket
    //     currentObject.localScale = targetScale;
    // }

    // private void OnSelectExited(SelectExitEventArgs args)
    // {
    //     if (currentObject != null)
    //     {
    //         // Restore the object's original scale
    //         currentObject.localScale = originalScale;
    //         currentObject = null;
    //     }
    // }
}
