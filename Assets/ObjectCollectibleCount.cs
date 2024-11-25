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
        text.text = $"{objectCount} / {maxObjects} objetos"; // Use maxObjects for the hardcoded max amount
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




// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ObjectCollectibleCount : MonoBehaviour
// {
//     TMPro.TMP_Text text;
//     int count;

//     void Awake()
//     {
//         text = GetComponent<TMPro.TMP_Text>();
//     }

//     void Start() // Fixed capitalization
//     {
//         UpdateCount(); // Fixed capitalization for consistency
//     }

//     void OnEnable()
//     {
//         ObjectCollectible.OnCollected += OnCollectibleCollected;
//     }

//     void OnDisable()
//     {
//         ObjectCollectible.OnCollected -= OnCollectibleCollected;
//     }

//     void OnCollectibleCollected()
//     {
//         count++;
//         UpdateCount();
//     }

//     void UpdateCount() // Fixed capitalization for consistency
//     {
//         text.text = $"{count} / {ObjectCollectible.total} objetos";
//     }
// }


// using System.Collections.Generic;
// using UnityEngine;

// public class CoinCollectibleCount : MonoBehaviour
// {
//     [Header("UI Elements")]
//     public TMPro.TMP_Text coinText;
//     public TMPro.TMP_Text objectText;

//     private Dictionary<CollectibleType, int> countByType = new Dictionary<CollectibleType, int>();

//     void Awake()
//     {
//         // Initialize counts for all types
//         foreach (CollectibleType type in System.Enum.GetValues(typeof(CollectibleType)))
//         {
//             countByType[type] = 0;
//         }
//     }

//     void Start()
//     {
//         UpdateUI();
//     }

//     void OnEnable()
//     {
//         ObjectCollectible.OnCollected += OnCollectibleCollected;
//     }

//     void OnDisable()
//     {
//         ObjectCollectible.OnCollected -= OnCollectibleCollected;
//     }

//     void OnCollectibleCollected(CollectibleType type)
//     {
//         if (countByType.ContainsKey(type))
//         {
//             countByType[type]++;
//         }
//         UpdateUI();
//     }

//     void UpdateUI()
//     {
//         // Update each UI element with the counts
//         coinText.text = $"monedas: {countByType[CollectibleType.Coin]} / {CoinCollectible.totalByType[CollectibleType.Coin]}";
//         objectText.text = $"objetos: {countByType[CollectibleType.Object]} / {ObjectCollectible.totalByType[CollectibleType.Object]}";
//     }
// }
