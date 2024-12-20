using UnityEngine;
using UnityEngine.SceneManagement;


public class ParedInvisibleFinal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Aseg�rate de que tu jugador tenga la etiqueta "Player"
        {
            Debug.Log("Victoria: Entraste al b�nker.");
            SceneManager.LoadScene("Victoria");
        }
    }

}
