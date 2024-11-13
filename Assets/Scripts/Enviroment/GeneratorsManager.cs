using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorsManager : MonoBehaviour
{
    public static GeneratorsManager Instance { get; private set; }

    private int _totalGenerators;
    private int _activatedGenerators;

    // Lista de generadores, que se llenará automáticamente
    public List<GeneratorRepair> generators;

    public delegate void OnAllGeneratorsActivated();
    public event OnAllGeneratorsActivated AllGeneratorsActivated;

    private void Awake()
    {
        // Asegurarse de que solo haya una instancia de GeneratorsManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Llenar la lista de generadores con los generadores hijos
        generators = new List<GeneratorRepair>(GetComponentsInChildren<GeneratorRepair>());
        _totalGenerators = generators.Count;

        // Si hay generadores, subscribirse a los eventos de activación
        foreach (var generator in generators)
        {
            generator.OnGeneratorActivated += HandleGeneratorActivated;
        }
    }

    // Método que se ejecuta cuando se activa un generador
    public void HandleGeneratorActivated()
    {
        _activatedGenerators++;
        if (_activatedGenerators >= _totalGenerators)
        {
            AllGeneratorsActivated?.Invoke();
        }
    }

    // Obtener el porcentaje de generadores activados
    public float GetActiveGeneratorPercentage()
    {
        return (float)_activatedGenerators / _totalGenerators;
    }
}
