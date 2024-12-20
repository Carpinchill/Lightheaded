using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneratorManager : MonoBehaviour
{
    //FRANCO
    public static GeneratorManager Instance; 
    public int _activeGenerators = 0; 
    public int totalGenerators = 5;  

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    public void ActivateGenerator()
    {
        _activeGenerators++;
        Debug.Log($"Generadores activados: {_activeGenerators}/{totalGenerators}");

        
        if (_activeGenerators >= totalGenerators)
        {
            
        }
    }

    private void LoadVictoryScene()
    {
        Debug.Log("¡Todos los generadores activados!");
        
    }
} 