using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameInteractuable : MonoBehaviour
{
    public GameObject DestroyableWall;
    public void Desaparecer()
    {
        DestroyableWall.SetActive(false); // Desactiva la pared
        this.gameObject.SetActive(false);
    }
}
