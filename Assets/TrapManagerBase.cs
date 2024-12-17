using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrapManagerBase : MonoBehaviour
{
    public enum TrapType
    {
        Stun, // Inmoviliza al jugador
        SlowDown,   // Ralentiza al jugador
        Noise       // Hace ruido para atraer al enemigo
    }

    public TrapType trapType; // Tipo de trampa asignado en el Inspector

    private Dictionary<TrapType, string> trapEffects = new Dictionary<TrapType, string>();

    void Start()
    {
        // Inicializamos el diccionario con los efectos de cada trampa
        trapEffects.Add(TrapType.Stun, "Frena al jugador por 2 segundos.");
        trapEffects.Add(TrapType.SlowDown, "Ralentiza al jugador mientras está en la trampa.");
        trapEffects.Add(TrapType.Noise, "Genera ruido y atrae al enemigo.");
    }

    // Método para obtener el efecto de la trampa
    public string GetTrapEffect()
    {
        if (trapEffects.TryGetValue(trapType, out string effect))
        {
            return effect;
        }
        else
        {
            Debug.LogWarning("El tipo de trampa no está definido.");
            return "Sin efecto.";
        }
    }

}
