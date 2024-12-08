using UnityEngine;
using System;

public class CoinCollectible : MonoBehaviour
{
    public static event Action OnCollected;
    public AudioClip collectSound; // The sound to play when collected
    public static int total;

    [Header("Rotation Settings")]
    public Vector3 rotationAxis = Vector3.up; // Default rotation axis (Y-axis)
    public float rotationSpeed = 100f;        // Speed of rotation

    private static AudioSource globalAudioSource;

    void Awake()
    {
        total++;
        
        // Find or create a global AudioSource if not already available
        if (globalAudioSource == null)
        {
            GameObject audioSourceObject = new GameObject("GlobalAudioSource");
            globalAudioSource = audioSourceObject.AddComponent<AudioSource>();
            globalAudioSource.playOnAwake = false;
            DontDestroyOnLoad(audioSourceObject); // Persist through scenes
        }
    }

    void Update()
    {
        // Rotate the object around the specified axis at the specified speed
        transform.Rotate(rotationAxis.normalized, rotationSpeed * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play sound using the global audio source
            if (collectSound != null && globalAudioSource != null)
            {
                globalAudioSource.PlayOneShot(collectSound);
            }

            OnCollected?.Invoke();
            Destroy(gameObject); // Destroy the collectible
        }
    }
}


// using UnityEngine;
// using System.Collections.Generic; // Necessary for using Dictionary
// using System; // If you have other system-level features like Action


// public enum CollectibleType
// {
//     Coin,
//     Object,
//     SolarDisk
// }

// public class ObjectCollectible : MonoBehaviour
// {
//     public static event Action<CollectibleType> OnCollected; // Pass the type when collected
//     public static Dictionary<CollectibleType, int> totalByType = new Dictionary<CollectibleType, int>();

//     [Header("Collectible Settings")]
//     public CollectibleType collectibleType; // Type of collectible
//     public Vector3 rotationAxis = Vector3.up; // Default rotation axis (Y-axis)
//     public float rotationSpeed = 100f;        // Speed of rotation

//     void Awake()
//     {
//         if (!totalByType.ContainsKey(collectibleType))
//         {
//             totalByType[collectibleType] = 0;
//         }
//         totalByType[collectibleType]++;
//     }

//     void Update()
//     {
//         // Rotate the object around the specified axis at the specified speed
//         transform.Rotate(rotationAxis.normalized, rotationSpeed * Time.deltaTime, Space.Self);
//     }

//     void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             OnCollected?.Invoke(collectibleType); // Pass the collectible type
//             Destroy(gameObject);
//         }
//     }
// }
