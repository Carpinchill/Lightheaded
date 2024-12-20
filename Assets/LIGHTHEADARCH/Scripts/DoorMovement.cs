using System.Collections;
using UnityEngine;

public class DoorMovement : MonoBehaviour
{
    public Animator Door;
    public Transform player; // Arrastra el jugador aquí desde el Inspector
    public float activationDistance = 5f; // Distancia máxima para activar la puerta
    private bool isDoorOpen = false;
    private bool Sonidox=true;

    // Referencias para el audio
    public AudioSource doorSound; // AudioSource asignado en el Inspector

    // Referencias para los GameObjects
    public GameObject objectToActivate; // El objeto que se activará
    public GameObject objectToDeactivate; // El objeto que se desactivará

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        // Verificar si el jugador está cerca
        if (distance <= activationDistance && !isDoorOpen /*&& GeneratorManager.Instance.totalGenerators == GeneratorManager.Instance._activeGenerators*/)
        {
            OpenDoor();
        }
        else if (distance > activationDistance && isDoorOpen)
        {
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        Door.Play("abrir");
        if(Sonidox)
        {
            PlaySound();
        }
         // Reproducir sonido
        isDoorOpen = true;
        Sonidox= false;

        // Activar y desactivar objetos
        if (objectToActivate != null) objectToActivate.SetActive(true);
        if (objectToDeactivate != null) objectToDeactivate.SetActive(false);
    }

    private void CloseDoor()
    {
        if (Sonidox)
        {
            PlaySound();
            Sonidox = false;
        }
        Door.Play("cerrar");
       
      

        // Activar y desactivar objetos
        if (objectToActivate != null) objectToActivate.SetActive(false);
        if (objectToDeactivate != null) objectToDeactivate.SetActive(true);
    }

    // Método para reproducir el sonido solo si no está sonando
    private void PlaySound()
    {
        if (!doorSound.isPlaying)
        {
            doorSound.Play();
        }
    }
}
