using UnityEngine;

public class SphereIndicatorControllerVR : MonoBehaviour
{
    public Transform npcTransform;         // NPC's Transform
    public Camera mainCamera;              // Camera under the XR Rig (or the correct VR camera)
    public float distanceFromCamera = 2f;  // Distance for the indicator sphere
    public bool alwaysVisible = true;      // Toggle visibility for testing

    void Update()
    {
        if (alwaysVisible)
        {
            // Ensure indicator sphere is visible during testing
            gameObject.SetActive(true);
        }

        // Calculate direction to the NPC relative to the camera
        Vector3 directionToNPC = npcTransform.position - mainCamera.transform.position;
        Vector3 cameraForward = mainCamera.transform.forward;

        // Check if the NPC is in front of the camera
        bool isInFront = Vector3.Dot(cameraForward, directionToNPC) > 0;

        if (!isInFront)
        {
            // Position the sphere in front of the camera at the specified distance
            Vector3 indicatorPosition = mainCamera.transform.position + cameraForward * distanceFromCamera;
            transform.position = indicatorPosition;

            // Make the sphere face the NPC's direction
            transform.LookAt(npcTransform);
        }
        else
        {
            // Optionally, you can hide the indicator when the NPC is in front of the camera
            gameObject.SetActive(false);
        }
    }
}
