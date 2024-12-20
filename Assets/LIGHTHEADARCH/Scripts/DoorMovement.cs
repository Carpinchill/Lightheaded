using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMovement : MonoBehaviour
{

    public Animator Door;
    public Transform player; // Arrastra el jugador aqu� desde el Inspector
    public float activationDistance = 5f; // Distancia m�xima para activar la puerta
    private bool isDoorOpen = false;

    // Referencias para el audio
    public AudioSource doorSound; // AudioSource asignado en el Inspector

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        // Verificar si el jugador est� cerca
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

    // M�todo para reproducir sonido por una duraci�n espec�fica
    private void PlaySoundForDuration(float duration)
    {
        if (!doorSound.isPlaying) // Solo reproducir si no est� sonando
        {
            doorSound.Play();
            StartCoroutine(StopSoundAfterDuration(duration));
        }
    }

    // Coroutine para detener el sonido despu�s de un tiempo
    private IEnumerator StopSoundAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        doorSound.Stop();
    }




}
