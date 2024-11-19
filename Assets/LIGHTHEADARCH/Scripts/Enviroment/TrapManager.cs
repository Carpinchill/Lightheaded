using System.Collections;
using UnityEngine;

public abstract class TrapManager : MonoBehaviour
{
    public abstract void Activate(Collider other); // M�todo abstracto para que lo implementen las clases derivadas

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Activate(other); // Llamar al m�todo espec�fico de cada trampa
        }
    }
}
