using System.Collections;
using UnityEngine;

public class LightResource : MonoBehaviour
{
    private bool _isRegenerating = false;
    public float regenRate = 0.1f; // Tasa de regeneraci�n por segundo    
    public float maxShine = 100f; // M�xima cantidad de luz (100)
    public float intensityMax = 2.0f; // Intensidad m�xima deseada
    public float maxLightRange = 10f; // Rango m�ximo de la luz
    [SerializeField]
    private float _ResidualLight; // Constante para luz residual m�nima 
    public float currentShine; // Cantidad de luz actual
    public Light lampLight; // Referencia a la luz de la l�mpara

    void Start()
    {
        if (lampLight == null)
        {
            lampLight = GetComponentInChildren<Light>(); // Suponiendo que la l�mpara est� como hijo
        }

        currentShine = maxShine; // Inicia la luz actual al m�ximo
        UpdateLightProperties(); // Actualiza intensidad y rango de la l�mpara al inicio
    }

    void Update()
    {
        // Asegurarse de que la intensidad y el rango se actualicen en cada frame
        UpdateLightProperties();
    }

    // Comienza la regeneraci�n de luz
    public void StartRegen()
    {
        if (!_isRegenerating && currentShine < maxShine)
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
        while (currentShine < maxShine && _isRegenerating)
        {
            currentShine += regenRate * Time.deltaTime;
            currentShine = Mathf.Clamp(currentShine, 0f, maxShine); // Limitar el valor a maxShine
            yield return null;
        }
    }

    // M�todo para recargar luz manualmente
    public void RechargeShine(float amount)
    {
        currentShine += amount;
        currentShine = Mathf.Clamp(currentShine, 0f, maxShine); // Limitar el valor a maxShine
        UpdateLightProperties();
    }

    // M�todo para reducir la luz (usado cuando la linterna est� activa)
    public void UseShine(float amount)
    {
        currentShine -= amount;
        currentShine = Mathf.Clamp(currentShine, 0f, maxShine); // Limitar el valor a 0
        UpdateLightProperties();
    }

    // M�todo para actualizar tanto la intensidad como el rango de la l�mpara
    private void UpdateLightProperties()
    {
        if (lampLight != null)
        {
            // Actualizar la intensidad
            lampLight.intensity = intensityMax * ((currentShine + _ResidualLight) / (maxShine + _ResidualLight));

            // Actualizar el rango de la luz
            lampLight.range = Mathf.Lerp(0f, maxLightRange, currentShine / maxShine);
        }
    }

    // Obtener la cantidad de luz actual
    public float GetCurrentShine()
    {
        return currentShine;
    }
}
