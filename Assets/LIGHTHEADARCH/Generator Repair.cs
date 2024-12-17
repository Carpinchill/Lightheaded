using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Necesario para trabajar con UI, incluyendo Image

public class GeneratorRepair : MonoBehaviour
{
    public ProgressBarHandler progressBarHandler; // Composición con barra de progreso
    public LightController lightController;       // Composición con controlador de luces
    public GameObject objectToDeactivate;         // Objeto a desactivar
    public AudioSource generatorActivateSound;    // Sonido del generador
    public float progressSpeed = 0.5f;            // Velocidad de llenado de la barra

    private bool isNearGenerator = false;
    private bool isActivated = false;

    void Update()
    {
        if (isNearGenerator && !isActivated)
        {
            if (Input.GetKey(KeyCode.E))
            {
                progressBarHandler.UpdateProgress(Time.deltaTime * progressSpeed);
            }

            if (progressBarHandler.IsComplete())
            {
                ActivateGenerator();
            }
        }
    }

    private void ActivateGenerator()
    {
        isActivated = true;
        progressBarHandler.ShowProgressBar(false);

        if (generatorActivateSound != null)
        {
            generatorActivateSound.Play();
        }

        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
        }

        if (lightController != null)
        {
            lightController.ToggleLights();
        }

        // Notifica al GeneratorManager
        if (GeneratorManager.Instance != null)
        {
            GeneratorManager.Instance.ActivateGenerator();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearGenerator = true;
            progressBarHandler.ShowProgressBar(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearGenerator = false;
            progressBarHandler.ShowProgressBar(false);
        }
    }

}