using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnTrigger : MonoBehaviour
{
    public string sceneName; // Nombre de la escena a cargar

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entra es el jugador (usando la etiqueta "Player")
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName); // Cargar la escena con el nombre especificado
        }
    }
}
