using UnityEngine;

public class ProximityScaler : MonoBehaviour
{
    public Vector3 normalScale = Vector3.one;         // Original scale
    public Vector3 scaledUpScale = Vector3.one * 1.5f; // Target scale when player is near
    public float scaleSpeed = 2f;                     // Speed of scaling transition

    private bool isNear = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered has the "Player" tag
        if (other.CompareTag("Player"))
        {
            isNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object that exited has the "Player" tag
        if (other.CompareTag("Player"))
        {
            isNear = false;
        }
    }

    void Update()
    {
        // Smoothly scale up or down based on proximity
        Vector3 targetScale = isNear ? scaledUpScale : normalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
    }
}
