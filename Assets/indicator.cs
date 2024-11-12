using UnityEngine;

public class SphereIndicatorController : MonoBehaviour
{
    public Transform npcTransform;     // NPC's position
    public Camera mainCamera;          // Player's camera
    public Transform indicatorSphere;  // 3D sphere indicator
    public float distanceFromCamera = 2f; // Distance to place the sphere in front of the camera

    void Update()
    {
        Vector3 directionToNPC = npcTransform.position - mainCamera.transform.position;
        Vector3 cameraForward = mainCamera.transform.forward;

        // Check if NPC is in front of the camera
        bool isInFront = Vector3.Dot(cameraForward, directionToNPC) > 0;

        if (!isInFront)
        {
            // Make the indicator visible
            indicatorSphere.gameObject.SetActive(true);

            // Calculate the position to place the sphere in front of the camera
            Vector3 indicatorPosition = mainCamera.transform.position + cameraForward * distanceFromCamera;
            indicatorSphere.position = indicatorPosition;

            // Rotate sphere to point toward the NPC
            indicatorSphere.LookAt(npcTransform);
        }
        else
        {
            // Hide the indicator if NPC is in view
            indicatorSphere.gameObject.SetActive(false);
        }
    }
}
