// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class ChangeSceneOnTrigger : MonoBehaviour
// {
//     public string sceneName; // Nombre de la escena a cargar

//     private void OnTriggerEnter(Collider other)
//     {
//         // Verificar si el objeto que entra es el jugador (usando la etiqueta "Player")
//         if (other.CompareTag("Player"))
//         {
//             SceneManager.LoadScene(sceneName); // Cargar la escena con el nombre especificado
//         }
//     }
// }


using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnTrigger : MonoBehaviour
{
    public string sceneName; // The name of the scene to load

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CleanupBeforeSceneLoad();
            SceneManager.LoadScene(sceneName); // Load the specified scene
        }
    }

    private void CleanupBeforeSceneLoad()
    {
        // Clean up any persistent objects (like GlobalAudioSource)
        GameObject globalAudioSourceObject = GameObject.Find("GlobalAudioSource");
        if (globalAudioSourceObject != null)
        {
            Destroy(globalAudioSourceObject);
        }
    }
}
