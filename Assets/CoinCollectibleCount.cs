using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinCollectibleCount : MonoBehaviour
{
    // Static variables to store the counts for the current scene
    public static int coinCount = 0;  // Static count for coins
    public static int totalCoins = 0; // Static total count for coins

    // Parameter for hardcoded max coins in this scene
    public int maxCoins = 5; // Set this in the Inspector to specify the maximum number of coins in the scene

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
        CoinCollectible.OnCollected += OnCollectibleCollected;
    }

    void OnDisable()
    {
        CoinCollectible.OnCollected -= OnCollectibleCollected;
    }

    // This method will be called when a coin is collected
    void OnCollectibleCollected()
    {
        coinCount++;
        UpdateCount();
    }

    // This method updates the UI text to display the current and total count
    void UpdateCount()
    {
        text.text = $"Monedas: {coinCount} / {maxCoins}"; // Use maxCoins for the hardcoded max amount
    }

    // Reset the coin count when a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        coinCount = 0; // Reset coin count for the new scene
        totalCoins = 0; // Reset the total count as well
        UpdateCount(); // Update the display to show the reset count
    }

    // Optionally, you can expose a method to change the max coin count dynamically if needed
    public void SetMaxCoins(int max)
    {
        maxCoins = max;
    }
}




// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CoinCollectibleCount : MonoBehaviour
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
//         CoinCollectible.OnCollected += OnCollectibleCollected;
//     }

//     void OnDisable()
//     {
//         CoinCollectible.OnCollected -= OnCollectibleCollected;
//     }

//     void OnCollectibleCollected()
//     {
//         count++;
//         UpdateCount();
//     }

//     void UpdateCount() // Fixed capitalization for consistency
//     {
//         text.text = $"{count} / {CoinCollectible.total} monedas";
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
