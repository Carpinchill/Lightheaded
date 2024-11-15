using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Necesario para trabajar con UI, incluyendo Image

public class GeneratorRepair : MonoBehaviour
{
    public Image progressBar; // Asigna la imagen que representa la barra de progreso
    public Transform generatorTransform; // Referencia al transform del generador
    public Vector3 offset = new Vector3(0, 2, 0); // Ajusta el offset para que esté sobre el generador
    public GameObject objectToDeactivate; // Objeto que se desactivará al completar la barra
    public Light lightToDeactivate; // Luz que se desactivará al completar la barra
    public Light lightToActivate;   // Luz que se activará al completar la barra
    [SerializeField]
    private bool isNearGenerator = false;
    private float progress = 0f; // Almacena el progreso actual
    public float progressSpeed = 0.5f; // Velocidad de llenado de la barra

    void Update()
    {
        if (isNearGenerator && progress < 1)
        {
            // Actualiza la posición del slider para que siga al generador
            if (Input.GetKey(KeyCode.E))
            {
                progress += Time.deltaTime * progressSpeed;
                progressBar.fillAmount = progress; // Actualiza la barra visualmente
            }
        }

        // Asegúrate de que el progreso no exceda 1
        progress = Mathf.Clamp01(progress);

        // Cuando el progreso está completo
        if (progress >= 1)
        {
            progressBar.gameObject.SetActive(false); // Oculta la barra

            if (objectToDeactivate != null)
            {
                objectToDeactivate.SetActive(false); // Desactiva el objeto
            }

            // Desactivar la primera luz y activar la segunda luz
            if (lightToDeactivate != null)
            {
                lightToDeactivate.enabled = false; // Desactiva la luz
            }

            if (lightToActivate != null)
            {
                lightToActivate.enabled = true;  // Activa la luz
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearGenerator = true;
            progressBar.gameObject.SetActive(true); // Muestra la barra de progreso
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearGenerator = false;
            progressBar.gameObject.SetActive(false); // Oculta la barra al salir del rango
        }
    }


}
