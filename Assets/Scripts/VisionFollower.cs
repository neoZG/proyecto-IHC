using UnityEngine;

public class SubtitlePlaneFollower : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;  // Reference to the main camera
    [SerializeField] private float distance = 2.0f;      // Distance from the camera to the plane
    [SerializeField] private Vector3 offset = new Vector3(0, -0.5f, 0);  // Offset for positioning

    private void Update()
    {
        FollowCamera();
    }

    private void FollowCamera()
    {
        // Set the plane position to be at the specified distance from the camera
        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * distance + offset;
        
        // Update plane position
        transform.position = targetPosition;

        // Rotate to always face the camera
        transform.LookAt(cameraTransform);
        transform.Rotate(0, 180, 0); // Rotate 180 degrees to face the camera directly
    }
}
