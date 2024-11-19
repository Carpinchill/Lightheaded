using System.Collections;
using UnityEngine;

public abstract class TrapManager : MonoBehaviour
{
    public abstract void Activate(Collider other); // Método abstracto para que lo implementen las clases derivadas

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Activate(other); // Llamar al método específico de cada trampa
        }
    }
}
