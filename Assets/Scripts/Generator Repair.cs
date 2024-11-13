using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GeneratorRepair : MonoBehaviour
{
    public Image progressBar; // Asigna el slider de la UI aquí
    public Transform generatorTransform; // Referencia al transform del generador
    public Vector3 offset = new(0, 2, 0); // Ajusta el offset para que esté sobre el generador
    [SerializeField]
    private bool _isNearGenerator = false;
    private float _progress = 0f;
    public float progressSpeed = 0.5f; // Velocidad de llenado de la barra

    
    public Light generatorLight; // La luz del generador
    public float maxLightIntensity; // Intensidad máxima de la luz
    public float maxLightRange; // Alcance máximo de la luz (mayor rango)
    private float _currentLightIntensity = 0f; // Intensidad actual de la luz
    private float _currentLightRange = 0f; // Alcance actual de la luz

   
    private LightResource _lightResource; // Referencia al script de stats del jugador
    public float shineConsumptionRate = 0.1f; // Tasa de consumo de Shine

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _lightResource = player.GetComponent<LightResource>();
        }
    }

    void Update()
    {
        if (_isNearGenerator && _progress < 1)
        {
            // Actualiza la posición del slider para que siga al generador
            if (Input.GetKey(KeyCode.E))
            {
                _progress += Time.deltaTime * progressSpeed;

                // Consumo de Shine mientras se llena la barra
                if (_lightResource.currentShine > 0)
                {
                    _lightResource.currentShine -= Time.deltaTime * shineConsumptionRate;
                }

                // Actualiza la barra de progreso
                progressBar.fillAmount = _progress;

                // Aumenta la intensidad de la luz (ligeramente)
                _currentLightIntensity = Mathf.Lerp(0f, maxLightIntensity, _progress * 0.5f); // Intensidad aumentada lentamente

                // Aumenta el rango de la luz con más énfasis
                _currentLightRange = Mathf.Lerp(0f, maxLightRange, _progress);

                // Actualizar la luz del generador
                generatorLight.intensity = _currentLightIntensity;
                generatorLight.range = _currentLightRange;
            }

            // Resetear si se deja de mantener 'E'
            if (Input.GetKeyUp(KeyCode.E) && _progress < 1)
            {
                _progress = 0f;
                progressBar.fillAmount = 0f;
            }
        }

        // Cuando la barra se llena, el generador se activa
        if (_progress >= 1)
        {
            ActivateGenerator();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isNearGenerator = true;
            progressBar.gameObject.SetActive(true); // Muestra la barra de progreso
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isNearGenerator = false;
        }
    }

    private void ActivateGenerator()
    {
        // Aquí pondremos la lógica para que el generador esté activo, por ejemplo,
        // activar una zona segura, etc.
        // Ejemplo: Hacer que el generador emita una señal de luz o active un área de seguridad.
        Debug.Log("Generador activado! Zona segura creada.");
    }
}
