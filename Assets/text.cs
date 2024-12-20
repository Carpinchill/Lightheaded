using System.Collections;
using UnityEngine;
using TMPro;

public class Text : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // El componente de texto
    public string fullText = ""; // Texto con saltos de línea
    public float typingSpeed = 0.05f; // Velocidad de escritura (en segundos por carácter)
    public float additionalDelayAfterComma = 0.2f; // Tiempo adicional de espera después de una coma
    public GameObject objectToActivate; // El objeto que se activará cuando termine el texto

    private void Start()
    {
        textComponent.text = ""; // Asegúrate de que el texto inicial esté vacío
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        string[] lines = fullText.Split('\n'); // Divide el texto por los saltos de línea

        foreach (string line in lines)
        {
            // Escribe cada línea
            foreach (char letter in line)
            {
                textComponent.text += letter; // Añadir carácter por carácter

                // Si el carácter es una coma, añadir un poco más de espera
                if (letter == ',')
                {
                    yield return new WaitForSeconds(additionalDelayAfterComma); // Espera extra después de la coma
                }
                else
                {
                    yield return new WaitForSeconds(typingSpeed); // Espera normal entre caracteres
                }
            }

            // Después de escribir la línea, esperar un poco antes de pasar a la siguiente línea
            yield return new WaitForSeconds(1f); // Tiempo de espera entre líneas, si lo deseas modificarlo
        }

        // Una vez que se haya escrito todo el texto, activar el GameObject
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true); // Activar el objeto
        }
    }
}
