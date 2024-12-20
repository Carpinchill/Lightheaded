using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  

public class GeneratorRepair : MonoBehaviour
{
    //BELEN
    public ProgressBarHandler progressBarHandler; 
    public LightController lightController;       
    public GameObject objectToDeactivate;         
    public AudioSource generatorActivateSound;    
    public float progressSpeed = 0.5f;            
    private bool _isNearGenerator = false;
    private bool _isActivated = false;

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
    }

    private void ActivateGenerator()
    {
        _isActivated = true;
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isNearGenerator = false;
            progressBarHandler.ShowProgressBar(false);
        }
    }

}