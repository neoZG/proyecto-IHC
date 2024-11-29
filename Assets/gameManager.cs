using TMPro; // Import TextMeshPro namespace
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int maxLives = 3; // Total lives allowed
    public int currentLives;

    public TMP_Text livesText; // TextMeshPro object to display current lives
    public Transform respawnPoint; // Point to respawn the player
    public TMP_Text gameOverText; // TextMeshPro object for the "You Died" message
    public GameObject gameOverCanvas; // Canvas for the "Game Over" screen
    public TMP_Text statsText; // Text to display stats like survival time and coins
    public TMP_Text countdownText; // Text to display the countdown
    public GameObject enemiesParent; // Parent object containing all enemies

    public Canvas redOverlayCanvas; // Reference to the red overlay Canvas
    private CanvasGroup redOverlayCanvasGroup; // The CanvasGroup to control fade
    private bool isGameOver = false;

    public AudioClip deathSound; // AudioClip for death sound
    private AudioSource audioSource; // AudioSource to play the sound

    private float survivalTime; // Variable to track survival time
    private int coinsCollected; // Variable to track coins collected

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Access the CanvasGroup that should already be attached to the redOverlayCanvas
        redOverlayCanvasGroup = redOverlayCanvas.GetComponent<CanvasGroup>();
        if (redOverlayCanvasGroup == null)
        {
            Debug.LogError("CanvasGroup component missing from redOverlayCanvas! Please add it manually.");
        }

        // Initialize the audio source if it isn't already attached
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {
        currentLives = maxLives;
        survivalTime = 0f;
        coinsCollected = 0;

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false); // Hide the game over canvas at the start

        // Ensure the red overlay starts hidden
        if (redOverlayCanvas != null)
        {
            redOverlayCanvas.gameObject.SetActive(false);
        }

        // Update the lives display at the start
        UpdateLivesText();
    }

    void Update()
    {
        if (!isGameOver)
        {
            survivalTime += Time.deltaTime; // Increment the survival time if the game is not over
        }
    }

    // Call this when the player dies
    public void PlayerDied()
    {
        if (isGameOver) return;

        PlayDeathSound(); // Play death sound effect
        currentLives--;

        if (currentLives > 0)
        {
            // Update lives text and respawn player
            UpdateLivesText();
            RespawnPlayer();
        }
        else
        {
            // Game over logic
            // Update lives text and respawn player
            UpdateLivesText();
            GameOver();
        }
    }

    void RespawnPlayer()
    {
        // Activate red overlay and start the fade effect
        if (redOverlayCanvas != null)
        {
            redOverlayCanvas.gameObject.SetActive(true);
            StartCoroutine(ActivateRedScreen());
        }
        // Player.Instance.RespawnToStart(); // Reset player to respawn point
    }

    void GameOver()
    {
        // Disable all enemies
        if (enemiesParent != null)
        {
            foreach (Transform enemy in enemiesParent.transform)
            {
                enemy.gameObject.SetActive(false);
            }
        }

        // Activate red overlay and start the fade effect
        if (redOverlayCanvas != null)
        {
            redOverlayCanvas.gameObject.SetActive(true);
            StartCoroutine(ActivateRedScreen());
        }

        isGameOver = true;

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true); // Show the Game Over canvas

        if (gameOverText != null)
            gameOverText.text = "Has muerto!";

        // Show stats
        if (statsText != null)
        {
            statsText.text = $"Tiempo: {survivalTime:F2}s \nMonedas: {coinsCollected}";
        }

        // Start countdown to main menu
        // StartCoroutine(StartCountdownToMainMenu());
        StartCoroutine(StartCountdownToRetry());
    }

    // Coroutine to fade in the red overlay and then fade it out after 1 second
    IEnumerator ActivateRedScreen()
    {
        float fadeTime = 1f;
        float elapsedTime = 0f;

        // Fade in (increase alpha to 1)
        while (elapsedTime < fadeTime)
        {
            redOverlayCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeTime);
            elapsedTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime to ignore time scale
            yield return null;
        }

        redOverlayCanvasGroup.alpha = 1f;

        // Wait for 1 second before fading out
        yield return new WaitForSecondsRealtime(1f);

        // Fade out (decrease alpha to 0)
        elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            redOverlayCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        redOverlayCanvasGroup.alpha = 0f;

        // Deactivate red overlay after fading out
        redOverlayCanvas.gameObject.SetActive(false);
    }

    // // Countdown coroutine to go to main menu
    // IEnumerator StartCountdownToMainMenu()
    // {
    //     float countdown = 10f; // Start countdown from 10 seconds

    //     while (countdown > 0)
    //     {
    //         countdownText.text = "Volviendo al menu en: " + Mathf.Ceil(countdown);
    //         countdown -= Time.deltaTime; // Decrease countdown
    //         yield return null;
    //     }

    //     Time.timeScale = 1f; // Pause the game
    //     LoadMainMenu(); // After countdown ends, load the main menu
    // }

    // Countdown coroutine to restart the scene
    IEnumerator StartCountdownToRetry()
    {
        float countdown = 10f;

        while (countdown > 0)
        {
            if (countdownText != null)
                countdownText.text = "Reiniciando en: " + Mathf.Ceil(countdown);

            countdown -= Time.deltaTime;
            yield return null;
        }

        Time.timeScale = 1f;
        StopAllCoroutines(); // Stop all coroutines to avoid lingering references
        RetryLevel();
    }

    // Method to restart the current level
    public void RetryLevel()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    // Call this method to return to the main menu
    public void LoadMainMenu()
    {
        // Destroy all objects to ensure a fresh start
        DestroyAllObjects();

        // Load the main menu scene
        SceneManager.LoadScene("0 Start Scene"); // Replace with your main menu scene name
    }

    // Destroy all objects in the current scene
    void DestroyAllObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj != this.gameObject && obj != gameOverCanvas && obj != redOverlayCanvas) 
            {
                Destroy(obj);
            }
        }
    }

    // Play the death sound effect
    void PlayDeathSound()
    {
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }

    // Update the lives display
    void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives.ToString();
        }
    }
}













// using TMPro; // Import TextMeshPro namespace
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using System.Collections;

// public class GameManager : MonoBehaviour
// {
//     public static GameManager Instance;

//     public int maxLives = 3; // Total lives allowed
//     public int currentLives;

//     public TMP_Text livesText; // TextMeshPro object to display current lives
//     public Transform respawnPoint; // Point to respawn the player
//     public TMP_Text gameOverText; // TextMeshPro object for the "You Died" message
//     public GameObject gameOverCanvas; // Canvas for the "Game Over" screen
//     public GameObject enemiesParent; // Parent object containing all enemies

//     public Canvas redOverlayCanvas; // Reference to the red overlay Canvas
//     private CanvasGroup redOverlayCanvasGroup; // The CanvasGroup to control fade
//     private bool isGameOver = false;

//     public AudioClip deathSound; // AudioClip for death sound
//     private AudioSource audioSource; // AudioSource to play the sound

//     void Awake()
//     {
//         // Singleton pattern
//         if (Instance == null)
//         {
//             Instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }

//         // Access the CanvasGroup that should already be attached to the redOverlayCanvas
//         redOverlayCanvasGroup = redOverlayCanvas.GetComponent<CanvasGroup>();
//         if (redOverlayCanvasGroup == null)
//         {
//             Debug.LogError("CanvasGroup component missing from redOverlayCanvas! Please add it manually.");
//         }

//         // Initialize the audio source if it isn't already attached
//         audioSource = gameObject.GetComponent<AudioSource>();
//         if (audioSource == null)
//         {
//             audioSource = gameObject.AddComponent<AudioSource>();
//         }
//     }

//     void Start()
//     {
//         currentLives = maxLives;
//         if (gameOverCanvas != null)
//             gameOverCanvas.SetActive(false); // Hide the game over canvas at the start

//         // Ensure the red overlay starts hidden
//         if (redOverlayCanvas != null)
//         {
//             redOverlayCanvas.gameObject.SetActive(false);
//         }

//         // Update the lives display at the start
//         UpdateLivesText();
//     }

//     // Call this when the player dies
//     public void PlayerDied()
//     {
//         if (isGameOver) return;

//         PlayDeathSound(); // Play death sound effect
//         currentLives--;

//         if (currentLives > 0)
//         {
//             // Update lives text and respawn player
//             UpdateLivesText();
//             RespawnPlayer();
//         }
//         else
//         {
//             // Game over logic
//             GameOver();
//         }
//     }

//     void RespawnPlayer()
//     {
//         // Activate red overlay and start the fade effect
//         if (redOverlayCanvas != null)
//         {
//             redOverlayCanvas.gameObject.SetActive(true);
//             StartCoroutine(ActivateRedScreen());
//         }
//         // Player.Instance.RespawnToStart(); // Reset player to respawn point
//     }

//     void GameOver()
//     {
//         isGameOver = true;

//         if (gameOverCanvas != null)
//             gameOverCanvas.SetActive(true); // Show the Game Over canvas

//         if (gameOverText != null)
//             gameOverText.text = "Has muerto!"; // Display "You Died!"

        // // Disable all enemies
        // if (enemiesParent != null)
        // {
        //     foreach (Transform enemy in enemiesParent.transform)
        //     {
        //         enemy.gameObject.SetActive(false);
        //     }
        // }

//         // Activate red overlay and start the fade effect
//         if (redOverlayCanvas != null)
//         {
//             redOverlayCanvas.gameObject.SetActive(true);
//             StartCoroutine(ActivateRedScreen());
//         }

//         // Time.timeScale = 0f; // Pause the game
//     }

//     // Coroutine to fade in the red overlay and then fade it out after 1 second
//     IEnumerator ActivateRedScreen()
//     {
//         // Fade in the red screen
//         float fadeTime = 1f; // Time to fade in
//         float elapsedTime = 0f;

//         // Fade in (increase alpha to 1)
//         while (elapsedTime < fadeTime)
//         {
//             redOverlayCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeTime);
//             elapsedTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime to ignore time scale
//             yield return null;
//         }

//         redOverlayCanvasGroup.alpha = 1f; // Ensure it's fully visible

//         // Wait for 1 second before fading out
//         yield return new WaitForSecondsRealtime(1f);

//         // Fade out (decrease alpha to 0)
//         elapsedTime = 0f;
//         while (elapsedTime < fadeTime)
//         {
//             redOverlayCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
//             elapsedTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime to ignore time scale
//             yield return null;
//         }

//         redOverlayCanvasGroup.alpha = 0f; // Ensure it's fully hidden

//         // Deactivate red overlay after fading out
//         redOverlayCanvas.gameObject.SetActive(false);
//     }

//     // Call this method when the player clicks "Retry"
//     public void Retry()
//     {
//         // Reset game state
//         currentLives = maxLives;
//         isGameOver = false;
//         // Time.timeScale = 1f; // Resume time

//         if (gameOverCanvas != null)
//             gameOverCanvas.SetActive(false);

//         // Reactivate enemies
//         if (enemiesParent != null)
//         {
//             foreach (Transform enemy in enemiesParent.transform)
//             {
//                 enemy.gameObject.SetActive(true);
//             }
//         }

//         RespawnPlayer();
//         // Reload the current scene to reset everything
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//     }

//     // Call this method to return to the main menu
//     public void LoadMainMenu()
//     {
//         // Destroy all objects to ensure a fresh start
//         DestroyAllObjects();

//         // Resume time and load the main menu scene
//         // Time.timeScale = 1f; // Resume time
//         SceneManager.LoadScene("0 Start Scene"); // Replace with your main menu scene name
//     }

//     // Destroy all objects in the current scene
//     void DestroyAllObjects()
//     {
//         GameObject[] allObjects = FindObjectsOfType<GameObject>();

//         foreach (GameObject obj in allObjects)
//         {
//             if (obj != this.gameObject) // Don't destroy the GameManager itself
//             {
//                 Destroy(obj);
//             }
//         }
//     }

//     // Play the death sound effect
//     void PlayDeathSound()
//     {
//         if (deathSound != null && audioSource != null)
//         {
//             audioSource.PlayOneShot(deathSound);
//         }
//     }

//     // Update the lives display
//     void UpdateLivesText()
//     {
//         if (livesText != null)
//         {
//             livesText.text = "Lives: " + currentLives.ToString();
//         }
//     }
// }


















// using TMPro; // Import TextMeshPro namespace
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using System.Collections;

// public class GameManager : MonoBehaviour
// {
//     public static GameManager Instance;

//     public int maxLives = 3; // Total lives allowed
//     public int currentLives;

//     public TMP_Text livesText; // TextMeshPro object to display current lives
//     public Transform respawnPoint; // Point to respawn the player
//     public TMP_Text gameOverText; // TextMeshPro object for the "You Died" message
//     public GameObject gameOverCanvas; // Canvas for the "Game Over" screen
//     public GameObject enemiesParent; // Parent object containing all enemies

//     public Canvas redOverlayCanvas; // Reference to the red overlay Canvas
//     private CanvasGroup redOverlayCanvasGroup; // The CanvasGroup to control fade
//     private bool isGameOver = false;

//     public AudioClip deathSound; // AudioClip for death sound
//     private AudioSource audioSource; // AudioSource to play the sound

//     void Awake()
//     {
//         // Singleton pattern
//         if (Instance == null)
//         {
//             Instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }

//         // Access the CanvasGroup that should already be attached to the redOverlayCanvas
//         redOverlayCanvasGroup = redOverlayCanvas.GetComponent<CanvasGroup>();
//         if (redOverlayCanvasGroup == null)
//         {
//             Debug.LogError("CanvasGroup component missing from redOverlayCanvas! Please add it manually.");
//         }

//         // Initialize the audio source if it isn't already attached
//         audioSource = gameObject.GetComponent<AudioSource>();
//         if (audioSource == null)
//         {
//             audioSource = gameObject.AddComponent<AudioSource>();
//         }
//     }

//     void Start()
//     {
//         currentLives = maxLives;
//         if (gameOverCanvas != null)
//             gameOverCanvas.SetActive(false); // Hide the game over canvas at the start

//         // Ensure the red overlay starts hidden
//         if (redOverlayCanvas != null)
//         {
//             redOverlayCanvas.gameObject.SetActive(false);
//         }

//         // Update the lives display at the start
//         UpdateLivesText();
//     }

//     // Call this when the player dies
//     public void PlayerDied()
//     {
//         if (isGameOver) return;

//         PlayDeathSound(); // Play death sound effect
//         currentLives--;

//         if (currentLives > 0)
//         {
//             // Update lives text and respawn player
//             UpdateLivesText();
//             RespawnPlayer();
//         }
//         else
//         {
//             // Game over logic
//             GameOver();
//         }
//     }

//     void RespawnPlayer()
//     {
//         // Activate red overlay and start the fade effect
//         if (redOverlayCanvas != null)
//         {
//             redOverlayCanvas.gameObject.SetActive(true);
//             StartCoroutine(ActivateRedScreen());
//         }
//         // Player.Instance.RespawnToStart(); // Reset player to respawn point
//     }

//     void GameOver()
//     {
//         isGameOver = true;

//         if (gameOverCanvas != null)
//             gameOverCanvas.SetActive(true); // Show the Game Over canvas

//         if (gameOverText != null)
//             gameOverText.text = "Has muerto!";

//         // Disable all enemies
//         if (enemiesParent != null)
//         {
//             foreach (Transform enemy in enemiesParent.transform)
//             {
//                 enemy.gameObject.SetActive(false);
//             }
//         }

//         // Activate red overlay and start the fade effect
//         if (redOverlayCanvas != null)
//         {
//             redOverlayCanvas.gameObject.SetActive(true);
//             StartCoroutine(ActivateRedScreen());
//         }

//         Time.timeScale = 0f; // Pause the game
//     }

//     // Coroutine to fade in the red overlay and then fade it out after 1 second
//     IEnumerator ActivateRedScreen()
//     {
//         // Fade in the red screen
//         float fadeTime = 1f; // Time to fade in
//         float elapsedTime = 0f;

//         // Fade in (increase alpha to 1)
//         while (elapsedTime < fadeTime)
//         {
//             redOverlayCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeTime);
//             elapsedTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime to ignore time scale
//             yield return null;
//         }

//         redOverlayCanvasGroup.alpha = 1f; // Ensure it's fully visible

//         // Wait for 1 second before fading out
//         yield return new WaitForSecondsRealtime(1f);

//         // Fade out (decrease alpha to 0)
//         elapsedTime = 0f;
//         while (elapsedTime < fadeTime)
//         {
//             redOverlayCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
//             elapsedTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime to ignore time scale
//             yield return null;
//         }

//         redOverlayCanvasGroup.alpha = 0f; // Ensure it's fully hidden

//         // Deactivate red overlay after fading out
//         redOverlayCanvas.gameObject.SetActive(false);
//     }

//     // Call this method when the player clicks "Retry"
//     public void Retry()
//     {
//         // Reset game state
//         currentLives = maxLives;
//         isGameOver = false;
//         Time.timeScale = 1f; // Resume time

//         if (gameOverCanvas != null)
//             gameOverCanvas.SetActive(false);

//         // Reactivate enemies
//         if (enemiesParent != null)
//         {
//             foreach (Transform enemy in enemiesParent.transform)
//             {
//                 enemy.gameObject.SetActive(true);
//             }
//         }

//         RespawnPlayer();
//     }

//     // Call this method to return to the main menu
//     public void LoadMainMenu()
//     {
//         Time.timeScale = 1f; // Resume time
//         SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
//     }

//     // Play the death sound effect
//     void PlayDeathSound()
//     {
//         if (deathSound != null && audioSource != null)
//         {
//             audioSource.PlayOneShot(deathSound);
//         }
//     }

//     // Update the lives display
//     void UpdateLivesText()
//     {
//         if (livesText != null)
//         {
//             livesText.text = "Lives: " + currentLives.ToString();
//         }
//     }
// }




















// using TMPro; // Import TextMeshPro namespace
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using System.Collections;

// public class GameManager : MonoBehaviour
// {
//     public static GameManager Instance;

//     public int maxLives = 3; // Total lives allowed
//     public int currentLives;

//     public Transform respawnPoint; // Point to respawn the player
//     public TMP_Text gameOverText; // TextMeshPro object for the "You Died" message
//     public GameObject gameOverCanvas; // Canvas for the "Game Over" screen
//     public GameObject enemiesParent; // Parent object containing all enemies

//     public Canvas redOverlayCanvas; // Reference to the red overlay Canvas
//     private CanvasGroup redOverlayCanvasGroup; // The CanvasGroup to control fade
//     private bool isGameOver = false;

//     void Awake()
//     {
//         // Singleton pattern
//         if (Instance == null)
//         {
//             Instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }

//         // Access the CanvasGroup that should already be attached to the redOverlayCanvas
//         redOverlayCanvasGroup = redOverlayCanvas.GetComponent<CanvasGroup>();
//         if (redOverlayCanvasGroup == null)
//         {
//             Debug.LogError("CanvasGroup component missing from redOverlayCanvas! Please add it manually.");
//         }
//     }

//     void Start()
//     {
//         currentLives = maxLives;
//         if (gameOverCanvas != null)
//             gameOverCanvas.SetActive(false); // Hide the game over canvas at the start

//         // Ensure the red overlay starts hidden
//         if (redOverlayCanvas != null)
//         {
//             redOverlayCanvas.gameObject.SetActive(false);
//         }
//     }

//     // Call this when the player dies
//     public void PlayerDied()
//     {
//         if (isGameOver) return;

//         currentLives--;

//         if (currentLives > 0)
//         {
//             // Respawn player
//             RespawnPlayer();
//         }
//         else
//         {
//             // Game over logic
//             GameOver();
//         }
//     }

//     void RespawnPlayer()
//     {
//         // Activate red overlay and start the fade effect
//         if (redOverlayCanvas != null)
//         {
//             redOverlayCanvas.gameObject.SetActive(true);
//             StartCoroutine(ActivateRedScreen());
//         }
//         // Player.Instance.RespawnToStart(); // Reset player to respawn point
//     }

//     void GameOver()
//     {
//         isGameOver = true;

//         if (gameOverCanvas != null)
//             gameOverCanvas.SetActive(true); // Show the Game Over canvas

//         if (gameOverText != null)
//             gameOverText.text = "You Died!";

//         // Disable all enemies
//         if (enemiesParent != null)
//         {
//             foreach (Transform enemy in enemiesParent.transform)
//             {
//                 enemy.gameObject.SetActive(false);
//             }
//         }

//         // Activate red overlay and start the fade effect
//         if (redOverlayCanvas != null)
//         {
//             redOverlayCanvas.gameObject.SetActive(true);
//             StartCoroutine(ActivateRedScreen());
//         }

//         Time.timeScale = 0f; // Pause the game
//     }

//     // Coroutine to fade in the red overlay and then fade it out after 1 second
//     IEnumerator ActivateRedScreen()
//     {
//         // Fade in the red screen
//         float fadeTime = 1f; // Time to fade in
//         float elapsedTime = 0f;

//         // Fade in (increase alpha to 1)
//         while (elapsedTime < fadeTime)
//         {
//             redOverlayCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeTime);
//             elapsedTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime to ignore time scale
//             yield return null;
//         }

//         redOverlayCanvasGroup.alpha = 1f; // Ensure it's fully visible

//         // Wait for 1 second before fading out
//         yield return new WaitForSecondsRealtime(1f);

//         // Fade out (decrease alpha to 0)
//         elapsedTime = 0f;
//         while (elapsedTime < fadeTime)
//         {
//             redOverlayCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
//             elapsedTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime to ignore time scale
//             yield return null;
//         }

//         redOverlayCanvasGroup.alpha = 0f; // Ensure it's fully hidden

//         // Deactivate red overlay after fading out
//         redOverlayCanvas.gameObject.SetActive(false);
//     }

//     // Call this method when the player clicks "Retry"
//     public void Retry()
//     {
//         // Reset game state
//         currentLives = maxLives;
//         isGameOver = false;
//         Time.timeScale = 1f; // Resume time

//         if (gameOverCanvas != null)
//             gameOverCanvas.SetActive(false);

//         // Reactivate enemies
//         if (enemiesParent != null)
//         {
//             foreach (Transform enemy in enemiesParent.transform)
//             {
//                 enemy.gameObject.SetActive(true);
//             }
//         }

//         RespawnPlayer();
//     }

//     // Call this method to return to the main menu
//     public void LoadMainMenu()
//     {
//         Time.timeScale = 1f; // Resume time
//         SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
//     }
// }




// using TMPro; // Import TextMeshPro namespace
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using System.Collections;

// public class GameManager : MonoBehaviour
// {
//     public static GameManager Instance;

//     public int maxLives = 3; // Total lives allowed
//     public int currentLives;

//     public Transform respawnPoint; // Point to respawn the player
//     public TMP_Text gameOverText; // TextMeshPro object for the "You Died" message
//     public GameObject gameOverCanvas; // Canvas for the "Game Over" screen
//     public GameObject enemiesParent; // Parent object containing all enemies

//     public Canvas redOverlayCanvas; // Reference to the red overlay Canvas
//     private CanvasGroup redOverlayCanvasGroup; // The CanvasGroup to control fade
//     private bool isGameOver = false;

//     public AudioClip deathSound; // AudioClip for death sound
//     private AudioSource audioSource; // AudioSource to play the sound

//     void Awake()
//     {
//         // Singleton pattern
//         if (Instance == null)
//         {
//             Instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }

//         // Access the CanvasGroup that should already be attached to the redOverlayCanvas
//         redOverlayCanvasGroup = redOverlayCanvas.GetComponent<CanvasGroup>();
//         if (redOverlayCanvasGroup == null)
//         {
//             Debug.LogError("CanvasGroup component missing from redOverlayCanvas! Please add it manually.");
//         }

//         // Initialize the audio source if it isn't already attached
//         audioSource = gameObject.GetComponent<AudioSource>();
//         if (audioSource == null)
//         {
//             audioSource = gameObject.AddComponent<AudioSource>();
//         }
//     }

//     void Start()
//     {
//         currentLives = maxLives;
//         if (gameOverCanvas != null)
//             gameOverCanvas.SetActive(false); // Hide the game over canvas at the start

//         // Ensure the red overlay starts hidden
//         if (redOverlayCanvas != null)
//         {
//             redOverlayCanvas.gameObject.SetActive(false);
//         }
//     }

//     // Call this when the player dies
//     public void PlayerDied()
//     {
//         if (isGameOver) return;

//         PlayDeathSound(); // Play death sound effect
//         currentLives--;

//         if (currentLives > 0)
//         {
//             // Respawn player
//             RespawnPlayer();
//         }
//         else
//         {
//             // Game over logic
//             GameOver();
//         }
//     }

//     void RespawnPlayer()
//     {
//         // Activate red overlay and start the fade effect
//         if (redOverlayCanvas != null)
//         {
//             redOverlayCanvas.gameObject.SetActive(true);
//             StartCoroutine(ActivateRedScreen());
//         }
//         // Player.Instance.RespawnToStart(); // Reset player to respawn point
//     }

//     void GameOver()
//     {
//         isGameOver = true;

//         if (gameOverCanvas != null)
//             gameOverCanvas.SetActive(true); // Show the Game Over canvas

//         if (gameOverText != null)
//             gameOverText.text = "You Died!";

//         // Disable all enemies
//         if (enemiesParent != null)
//         {
//             foreach (Transform enemy in enemiesParent.transform)
//             {
//                 enemy.gameObject.SetActive(false);
//             }
//         }

//         // Activate red overlay and start the fade effect
//         if (redOverlayCanvas != null)
//         {
//             redOverlayCanvas.gameObject.SetActive(true);
//             StartCoroutine(ActivateRedScreen());
//         }

//         Time.timeScale = 0f; // Pause the game
//     }

//     // Coroutine to fade in the red overlay and then fade it out after 1 second
//     IEnumerator ActivateRedScreen()
//     {
//         // Fade in the red screen
//         float fadeTime = 1f; // Time to fade in
//         float elapsedTime = 0f;

//         // Fade in (increase alpha to 1)
//         while (elapsedTime < fadeTime)
//         {
//             redOverlayCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeTime);
//             elapsedTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime to ignore time scale
//             yield return null;
//         }

//         redOverlayCanvasGroup.alpha = 1f; // Ensure it's fully visible

//         // Wait for 1 second before fading out
//         yield return new WaitForSecondsRealtime(1f);

//         // Fade out (decrease alpha to 0)
//         elapsedTime = 0f;
//         while (elapsedTime < fadeTime)
//         {
//             redOverlayCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
//             elapsedTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime to ignore time scale
//             yield return null;
//         }

//         redOverlayCanvasGroup.alpha = 0f; // Ensure it's fully hidden

//         // Deactivate red overlay after fading out
//         redOverlayCanvas.gameObject.SetActive(false);
//     }

//     // Call this method when the player clicks "Retry"
//     public void Retry()
//     {
//         // Reset game state
//         currentLives = maxLives;
//         isGameOver = false;
//         Time.timeScale = 1f; // Resume time

//         if (gameOverCanvas != null)
//             gameOverCanvas.SetActive(false);

//         // Reactivate enemies
//         if (enemiesParent != null)
//         {
//             foreach (Transform enemy in enemiesParent.transform)
//             {
//                 enemy.gameObject.SetActive(true);
//             }
//         }

//         RespawnPlayer();
//     }

//     // Call this method to return to the main menu
//     public void LoadMainMenu()
//     {
//         Time.timeScale = 1f; // Resume time
//         SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
//     }

//     // Play the death sound effect
//     void PlayDeathSound()
//     {
//         if (deathSound != null && audioSource != null)
//         {
//             audioSource.PlayOneShot(deathSound);
//         }
//     }
// }
