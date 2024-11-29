// using UnityEngine;
// using System;
// using UnityEngine.SceneManagement;

// public class ObjectCollectible : MonoBehaviour
// {
//     public static event Action OnCollected;
//     public AudioClip collectSound; // The sound to play when collected
//     public static int total;

//     [Header("Rotation Settings")]
//     public Vector3 rotationAxis = Vector3.up; // Default rotation axis (Y-axis)
//     public float rotationSpeed = 100f;        // Speed of rotation
//     private static AudioSource globalAudioSource;

//     [Header("Scene Transition")]
//     public string nextSceneName; // Name of the next scene to load

//     void Awake()
//     {
//         total++;
//         // Find or create a global AudioSource if not already available
//         if (globalAudioSource == null)
//         {
//             GameObject audioSourceObject = new GameObject("GlobalAudioSource");
//             globalAudioSource = audioSourceObject.AddComponent<AudioSource>();
//             globalAudioSource.playOnAwake = false;
//             DontDestroyOnLoad(audioSourceObject); // Persist through scenes
//         }
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
//             // Play sound using the global audio source
//             if (collectSound != null && globalAudioSource != null)
//             {
//                 globalAudioSource.PlayOneShot(collectSound);
//                 float soundDuration = collectSound.length; // Get the duration of the sound
//                 Invoke(nameof(TransitionToNextScene), soundDuration); // Schedule transition after sound
//             }
//             else
//             {
//                 TransitionToNextScene(); // Immediate transition if no sound is set
//             }

//             OnCollected?.Invoke();
//             Destroy(gameObject); // Destroy the collectible
//         }
//     }

//     void TransitionToNextScene()
//     {
//         // Clean up objects in the scene
//         foreach (GameObject obj in FindObjectsOfType<GameObject>())
//         {
//             if (obj != globalAudioSource.gameObject) // Avoid destroying the audio source if it's persisting
//             {
//                 Destroy(obj);
//             }
//         }

//         // Load the next scene
//         SceneManager.LoadScene(nextSceneName);
//     }
// }








using UnityEngine;
using System;

public class ObjectCollectible : MonoBehaviour
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