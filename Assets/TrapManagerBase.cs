using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrapManagerBase : MonoBehaviour
{
    //BELEN
    public enum TrapType
    {
        Stun, 
        SlowDown,   
        Noise       
    }

    public TrapType trapType; 

    private Dictionary<TrapType, string> trapEffects = new Dictionary<TrapType, string>();

    void Start()
    {
        
        trapEffects.Add(TrapType.Stun, "Frena al jugador por 2 segundos.");
        trapEffects.Add(TrapType.SlowDown, "Ralentiza al jugador mientras está en la trampa.");
        trapEffects.Add(TrapType.Noise, "Genera ruido y atrae al enemigo.");
    }

    
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
