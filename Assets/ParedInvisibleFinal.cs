using UnityEngine;
using UnityEngine.SceneManagement;


public class ParedInvisibleFinal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que tu jugador tenga la etiqueta "Player"
        {
            LoadVictoryScene();
        }
    }
    private void LoadVictoryScene()
    {
        Debug.Log("¡Todos los generadores activados! Cargando escena de victoria...");
        Time.timeScale = 0;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Victoria");
    }

}
