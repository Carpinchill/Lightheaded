using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public GameObject lightsParent;  // El GameObject que contiene las luces hijas
    public float flickerDurationMin = 0.1f;  // Tiempo m�nimo para el parpadeo (0.1 segundos)
    public float flickerDurationMax = 0.5f;  // Tiempo m�ximo para el parpadeo (0.5 segundos)
    public float normalInterval = 5f;  // Duraci�n de estado normal (5 segundos)
    public float flickerInterval = 5f;  // Duraci�n del parpadeo (5 segundos)

    private float nextStateChangeTime;
    private bool isFlickering = false;
    private float nextFlickerTime;

    private void Start()
    {
        nextStateChangeTime = Time.time + normalInterval;  // Comienza con el estado normal
        lightsParent.SetActive(true);  // Asegurarse de que la luz inicie encendida
    }

    private void Update()
    {
        // Cambiar entre estados (normal o parpadeando)
        if (Time.time >= nextStateChangeTime)
        {
            isFlickering = !isFlickering;  // Alternar el estado
            nextStateChangeTime = Time.time + (isFlickering ? flickerInterval : normalInterval);

            if (isFlickering)
            {
                // Configurar el pr�ximo parpadeo solo cuando se entra en modo parpadeo
                nextFlickerTime = Time.time + Random.Range(flickerDurationMin, flickerDurationMax);
            }
            else
            {
                // Asegurarse de que la luz est� encendida al entrar en estado normal
                lightsParent.SetActive(true);
            }
        }

        // Si estamos en modo parpadeo, alternar el estado de la luz
        if (isFlickering && Time.time >= nextFlickerTime)
        {
            bool isActive = lightsParent.activeSelf;
            lightsParent.SetActive(!isActive);  // Cambiar estado (encender o apagar)
            nextFlickerTime = Time.time + Random.Range(flickerDurationMin, flickerDurationMax);
        }
    }
}
