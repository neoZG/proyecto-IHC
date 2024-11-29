// using System.Collections;
// using UnityEngine;
// using TMPro;
// using UnityEngine.SceneManagement;
// using System.Collections.Generic;

// public class ObjectCollectibleCount : MonoBehaviour
// {
//     // Static dictionary to store counts for each object type
//     public static Dictionary<ObjectType, int> objectCounts = new Dictionary<ObjectType, int>();

//     // Dictionary to store the maximum object count for each object type
//     public static Dictionary<ObjectType, int> maxObjects = new Dictionary<ObjectType, int>();

//     TMPro.TMP_Text text; // Reference to the TMP_Text component

//     void Awake()
//     {
//         text = GetComponent<TMPro.TMP_Text>();

//         // Reset counts when the scene is loaded
//         SceneManager.sceneLoaded += OnSceneLoaded;
//     }

//     void Start()
//     {
//         UpdateCount(ObjectType.Quipu); // Start by showing count for Quipu
//     }

//     void OnEnable()
//     {
//         ObjectCollectible.OnCollected += OnCollectibleCollected;
//     }

//     void OnDisable()
//     {
//         ObjectCollectible.OnCollected -= OnCollectibleCollected;
//     }

//     // This method will be called when an object is collected
//     void OnCollectibleCollected(ObjectType objectType)
//     {
//         if (objectCounts.ContainsKey(objectType))
//         {
//             objectCounts[objectType]++;
//         }
//         else
//         {
//             objectCounts[objectType] = 1; // Initialize count for new object type
//         }

//         UpdateCount(objectType); // Update the UI for the specific object type
//     }

//     // This method updates the UI text to display the current and total count
//     void UpdateCount(ObjectType objectType)
//     {
//         // Check if the object type has a max count set
//         if (maxObjects.ContainsKey(objectType))
//         {
//             int max = maxObjects[objectType];
//             text.text = $"{objectCounts[objectType]} / {max} {objectType.ToString()}"; // Display count for specific object type
//         }
//     }

//     // Reset the object counts when a new scene is loaded
//     void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//     {
//         objectCounts.Clear(); // Reset object counts for all types
//         UpdateCount(ObjectType.Quipu); // Optionally set which object to display first, or update all
//     }

//     // Optionally, you can expose a method to change the max object count dynamically if needed
//     public void SetMaxObjects(ObjectType objectType, int max)
//     {
//         if (!maxObjects.ContainsKey(objectType))
//         {
//             maxObjects[objectType] = max; // Set the max objects for the type
//         }
//     }
// }










using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class ObjectCollectibleCount : MonoBehaviour
{
    // Static variables to store the counts for the current scene
    public static int objectCount = 0;  // Static count for objects
    public static int totalObjects = 0; // Static total count for objects

    // Parameter for hardcoded max objects in this scene
    public int maxObjects = 3; // Set this in the Inspector to specify the maximum number of objects in the scene

    TMPro.TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();

        // Reset count when the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        UpdateCount(); // Update the text with the initial count
    }

    void OnEnable()
    {
        ObjectCollectible.OnCollected += OnCollectibleCollected;
    }

    void OnDisable()
    {
        ObjectCollectible.OnCollected -= OnCollectibleCollected;
    }

    // This method will be called when an object is collected
    void OnCollectibleCollected()
    {
        objectCount++;
        UpdateCount();
    }

    // This method updates the UI text to display the current and total count
    void UpdateCount()
    {
        text.text = $"Disco Solar {objectCount} / {maxObjects}"; // Use maxObjects for the hardcoded max amount
    }

    // Reset the object count when a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        objectCount = 0; // Reset object count for the new scene
        totalObjects = 0; // Reset the total count as well
        UpdateCount(); // Update the display to show the reset count
    }

    // Optionally, you can expose a method to change the max object count dynamically if needed
    public void SetMaxObjects(int max)
    {
        maxObjects = max;
    }
}


