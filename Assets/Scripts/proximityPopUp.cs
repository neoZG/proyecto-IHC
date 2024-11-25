// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;

// public class ProximityPopup : MonoBehaviour
// {
//     public GameObject canvasObject; // Drag your canvas here
//     private XRGrabInteractable grabInteractable;

//     private void Start()
//     {
//         // Hide the canvas initially
//         canvasObject.SetActive(false);
        
//         // Get the XRGrabInteractable component attached to the object
//         grabInteractable = GetComponent<XRGrabInteractable>();

//         if (grabInteractable != null)
//         {
//             // Subscribe to the grab and drop events
//             grabInteractable.onSelectEntered.AddListener(OnGrab);
//             grabInteractable.onSelectExited.AddListener(OnDrop);
//         }
//     }

//     private void OnGrab(XRBaseInteractor interactor)
//     {
//         // When the object is grabbed, show the canvas
//         canvasObject.SetActive(true);
//     }

//     private void OnDrop(XRBaseInteractor interactor)
//     {
//         // When the object is dropped, hide the canvas
//         canvasObject.SetActive(false);
//     }

//     private void OnDestroy()
//     {
//         // Unsubscribe from the events when the object is destroyed
//         if (grabInteractable != null)
//         {
//             grabInteractable.onSelectEntered.RemoveListener(OnGrab);
//             grabInteractable.onSelectExited.RemoveListener(OnDrop);
//         }
//     }

//     // Optionally, you can still retain proximity-based activation for when the player is near
//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("Player") && !grabInteractable.isSelected)
//         {
//             // Only show the tooltip when the player is near but not grabbing the object
//             canvasObject.SetActive(true);
//         }
//     }

//     private void OnTriggerExit(Collider other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             // Hide the tooltip if the player leaves the trigger area
//             if (!grabInteractable.isSelected) // Check if not grabbed
//             {
//                 canvasObject.SetActive(false);
//             }
//         }
//     }
// }





using UnityEngine;

public class ProximityPopup : MonoBehaviour
{
    public GameObject canvasObject; // Drag your canvas here

    private void Start()
    {
        // Hide the canvas initially
        canvasObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is entering the trigger area
        if (other.CompareTag("Player")) // Make sure to tag the player as "Player"
        {
            canvasObject.SetActive(true); // Show the canvas
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player is exiting the trigger area
        if (other.CompareTag("Player"))
        {
            canvasObject.SetActive(false); // Hide the canvas
        }
    }
}
