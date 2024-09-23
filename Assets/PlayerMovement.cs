using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento
    private Rigidbody rb;
    private bool isImmobilized = false; // Flag para verificar si está inmovilizado
    private float originalSpeed; // Almacenar la velocidad original

    void Start()
    {
        // Obtener el componente Rigidbody
        rb = GetComponent<Rigidbody>();
        originalSpeed = speed; // Guardar la velocidad original
    }

    void FixedUpdate()
    {
        if (!isImmobilized) // Solo mover si no está inmovilizado
        {
            // Leer las entradas del jugador (teclado WASD o flechas)
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            // Crear un vector de movimiento basado en las entradas
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            // Aplicar la velocidad de movimiento
            rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);
        }
    }

    public IEnumerator Immobilize(float duration)
    {
        isImmobilized = true; // Inmovilizar al jugador
        yield return new WaitForSeconds(duration); // Esperar el tiempo de inmovilización
        isImmobilized = false; // Liberar al jugador
    }

    public void ReduceSpeed()
    {
        speed = originalSpeed / 2; // Reducir la velocidad a la mitad
    }

    public void RestoreSpeed()
    {
        speed = originalSpeed; // Restaurar la velocidad original
    }

}
