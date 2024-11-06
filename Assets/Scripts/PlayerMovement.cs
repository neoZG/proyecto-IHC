using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento del jugador

    void Update()
    {
        // Obtener entrada horizontal y vertical
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Crear un vector de movimiento
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Aplicar movimiento al jugador
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}
