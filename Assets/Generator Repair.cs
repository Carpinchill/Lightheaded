using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Necesario para trabajar con UI, incluyendo Image

public class GeneratorRepair : MonoBehaviour
{
    public Image progressBar; // Asigna el slider de la UI aquí
    public Transform generatorTransform; // Referencia al transform del generador
    public Vector3 offset = new Vector3(0, 2, 0); // Ajusta el offset para que esté sobre el generador
    [SerializeField]
    private bool isNearGenerator = false;
    private float progress = 0f;
    public float progressSpeed = 0.5f; // Velocidad de llenado de la barra

    void Update()
    {

        if (isNearGenerator && progress < 1)
        {
            // Actualiza la posición del slider para que siga al generador

            if (Input.GetKey(KeyCode.E))
            {

                progress += Time.deltaTime * progressSpeed;
                progressBar.fillAmount = progress;



            }

            if (Input.GetKeyUp(KeyCode.E) && progress < 1)
            {
                progress = 0f;
                progressBar.fillAmount = 0f;
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


        }
    }


}
