using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneratorManager : MonoBehaviour
{
    public static GeneratorManager Instance; // Singleton
    private int activeGenerators = 0; // Contador de generadores activados
    public int totalGenerators = 5;  // Número total de generadores necesarios para ganar

    private void Awake()
    {
        // Configurar Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Llamado desde el script GeneratorRepair cuando un generador es activado
    public void ActivateGenerator()
    {
        activeGenerators++;
        Debug.Log($"Generadores activados: {activeGenerators}/{totalGenerators}");

        // Verifica si todos los generadores están activados
        if (activeGenerators >= totalGenerators)
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
        SceneManager.LoadScene(4); // Cambia "4" por el índice o nombre de tu escena de victoria
    }
}