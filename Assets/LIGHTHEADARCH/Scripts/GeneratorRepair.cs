using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GeneratorRepair : MonoBehaviour
{
    public ProgressBarHandler progressBarHandler;
    public LightController lightController;
    public GameObject objectToDeactivate;
    public AudioSource generatorActivateSound;
    public float progressSpeed = 0.5f;
    private bool _isNearGenerator = false;
    private bool _isActivated = false;

    public GameObject interactionCanvas;  // Referencia al Canvas interactivo
    public GameObject progressBarCanvas;  // Referencia al Canvas de la barra de progreso
    private Transform mainCamera;         // Referencia a la cámara principal

    void Start()
    {
        // Obtiene la cámara principal
        mainCamera = Camera.main.transform;

        // Asegúrate de que los Canvas estén desactivados al iniciar
        if (interactionCanvas != null)
        {
            interactionCanvas.SetActive(false);
        }

        if (progressBarCanvas != null)
        {
            progressBarCanvas.SetActive(false);
        }
    }

    void Update()
    {
        if (_isNearGenerator && !_isActivated)
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

        // Si los Canvas están activos, haz que miren hacia la cámara
        if (interactionCanvas != null && interactionCanvas.activeSelf)
        {
            interactionCanvas.transform.LookAt(mainCamera);
        }

        if (progressBarCanvas != null && progressBarCanvas.activeSelf)
        {
            progressBarCanvas.transform.LookAt(mainCamera);
        }
    }

    private void ActivateGenerator()
    {
        _isActivated = true;
        progressBarHandler.ShowProgressBar(false);

        if (interactionCanvas != null)
        {
            interactionCanvas.SetActive(false);
        }

        if (progressBarCanvas != null)
        {
            progressBarCanvas.SetActive(false);
        }

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

        if (GeneratorManager.Instance != null)
        {
            GeneratorManager.Instance.ActivateGenerator();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isNearGenerator = true;
            progressBarHandler.ShowProgressBar(true);

            if (interactionCanvas != null && !_isActivated)
            {
                interactionCanvas.SetActive(true);
            }

            if (progressBarCanvas != null && !_isActivated)
            {
                progressBarCanvas.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isNearGenerator = false;
            progressBarHandler.ShowProgressBar(false);

            if (interactionCanvas != null)
            {
                interactionCanvas.SetActive(false);
            }

            if (progressBarCanvas != null)
            {
                progressBarCanvas.SetActive(false);
            }
        }
    }



}