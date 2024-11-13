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
