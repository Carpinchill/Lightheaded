using System.Collections;
using UnityEngine;

public class LightResource : MonoBehaviour
{
    private bool _isRegenerating = false;
    public float regenRate = 0.1f; // Tasa de regeneraci�n por segundo

    public float maxShine = 100f; // M�xima cantidad de luz (100)
    public float intensityMax = 2.0f; // Intensidad m�xima deseada
    private float _ResidualLight = 0.1f; // Constante para luz residual m�nima

    [SerializeField]
    private float _currentShine; // Cantidad de luz actual

    public Light lampLight; // Referencia a la luz de la l�mpara

    void Start()
    {
        if (lampLight == null)
        {
            lampLight = GetComponentInChildren<Light>(); // Suponiendo que la l�mpara est� como hijo
        }

        _currentShine = maxShine; // Inicia la luz actual al m�ximo
        UpdateLightIntensity(); // Actualizar la intensidad de la l�mpara al inicio
    }

    void Update()
    {
        // Asegurarse de que la intensidad se actualice en cada frame
        UpdateLightIntensity();
    }

    // Comienza la regeneraci�n de luz
    public void StartRegen()
    {
        if (!_isRegenerating && _currentShine < maxShine)
        {
            _isRegenerating = true;
            StartCoroutine(RegenerateShine());
        }
    }

    // Detiene la regeneraci�n de luz
    public void StopRegen()
    {
        if (_isRegenerating)
        {
            _isRegenerating = false;
            StopCoroutine(RegenerateShine());
        }
    }

    // Regeneraci�n continua de la luz
    private IEnumerator RegenerateShine()
    {
        while (_currentShine < maxShine && _isRegenerating)
        {
            _currentShine += regenRate * Time.deltaTime;
            _currentShine = Mathf.Clamp(_currentShine, 0f, maxShine); // Limitar el valor a maxShine
            yield return null;
        }
    }

    // M�todo para recargar luz manualmente
    public void RechargeShine(float amount)
    {
        _currentShine += amount;
        _currentShine = Mathf.Clamp(_currentShine, 0f, maxShine); // Limitar el valor a maxShine
        UpdateLightIntensity();
    }

    // M�todo para reducir la luz (usado cuando la linterna est� activa)
    public void UseShine(float amount)
    {
        _currentShine -= amount;
        _currentShine = Mathf.Clamp(_currentShine, 0f, maxShine); // Limitar el valor a 0
        UpdateLightIntensity();
    }

    // M�todo para actualizar la intensidad de la l�mpara
    private void UpdateLightIntensity()
    {
        if (lampLight != null)
        {
            // Calcula la intensidad usando la f�rmula con una leve intensidad residual
            lampLight.intensity = intensityMax * ((_currentShine + _ResidualLight) / (maxShine + _ResidualLight));
        }
    }

    // Obtener la cantidad de luz actual
    public float GetCurrentShine()
    {
        return _currentShine;
    }
}
