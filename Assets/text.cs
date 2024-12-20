using System.Collections;
using UnityEngine;
using TMPro;

public class Text : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // El componente de texto
    public string fullText = ""; // Texto con saltos de l�nea
    public float typingSpeed = 0.05f; // Velocidad de escritura (en segundos por car�cter)
    public float additionalDelayAfterComma = 0.2f; // Tiempo adicional de espera despu�s de una coma
    public GameObject objectToActivate; // El objeto que se activar� cuando termine el texto

    private void Start()
    {
        textComponent.text = ""; // Aseg�rate de que el texto inicial est� vac�o
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        string[] lines = fullText.Split('\n'); // Divide el texto por los saltos de l�nea

        foreach (string line in lines)
        {
            // Escribe cada l�nea
            foreach (char letter in line)
            {
                textComponent.text += letter; // A�adir car�cter por car�cter

                // Si el car�cter es una coma, a�adir un poco m�s de espera
                if (letter == ',')
                {
                    yield return new WaitForSeconds(additionalDelayAfterComma); // Espera extra despu�s de la coma
                }
                else
                {
                    yield return new WaitForSeconds(typingSpeed); // Espera normal entre caracteres
                }
            }

            // Despu�s de escribir la l�nea, esperar un poco antes de pasar a la siguiente l�nea
            yield return new WaitForSeconds(1f); // Tiempo de espera entre l�neas, si lo deseas modificarlo
        }

        // Una vez que se haya escrito todo el texto, activar el GameObject
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true); // Activar el objeto
        }
    }
}
