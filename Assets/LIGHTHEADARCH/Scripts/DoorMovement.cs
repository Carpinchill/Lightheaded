using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMovement : MonoBehaviour
{

    public Animator Door;
    public Transform player; // Arrastra el jugador aquí desde el Inspector
    public float activationDistance = 5f; // Distancia máxima para activar la puerta
    private bool isDoorOpen = false;

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        // Verificar si el jugador está cerca y si todos los generadores están activados
        if (distance <= activationDistance && !isDoorOpen && GeneratorManager.Instance.totalGenerators == GeneratorManager.Instance._activeGenerators)
        {
            Door.Play("abrir");
            isDoorOpen = true;
        }
        else if (distance > activationDistance && isDoorOpen)
        {
            Door.Play("cerrar");
            isDoorOpen = false;
        }
    }



}
