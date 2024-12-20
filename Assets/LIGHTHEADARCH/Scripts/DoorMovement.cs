using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMovement : MonoBehaviour
{

    public Animator Door;
    public Transform player; // Arrastra el jugador aquí desde el Inspector
    public float activationDistance = 5f; // Distancia máxima para activar la puerta
    private bool isDoorOpen = false;

    // Referencias para el audio
    public AudioSource doorSound; // AudioSource asignado en el Inspector

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        // Verificar si el jugador está cerca
        if (distance <= activationDistance && !isDoorOpen && GeneratorManager.Instance.totalGenerators == GeneratorManager.Instance._activeGenerators)
        {
            Door.Play("abrir");
            PlaySoundForDuration(5f); // Reproducir sonido por 5 segundos
            isDoorOpen = true;
        }
        else if (distance > activationDistance && isDoorOpen)
        {
            Door.Play("cerrar");
            PlaySoundForDuration(5f); // Reproducir sonido por 5 segundos
            isDoorOpen = false;
        }
    }

    // Método para reproducir sonido por una duración específica
    private void PlaySoundForDuration(float duration)
    {
        if (!doorSound.isPlaying) // Solo reproducir si no está sonando
        {
            doorSound.Play();
            StartCoroutine(StopSoundAfterDuration(duration));
        }
    }

    // Coroutine para detener el sonido después de un tiempo
    private IEnumerator StopSoundAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        doorSound.Stop();
    }




}
