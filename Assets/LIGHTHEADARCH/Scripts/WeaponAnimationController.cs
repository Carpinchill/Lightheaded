using UnityEngine;

public class WeaponAnimationController : MonoBehaviour
{
    //FRANCO
    private Weapons weapons;  // Referencia al script Weapons

    void Start()
    {
        // Obtener la referencia al script Weapons
        weapons = GetComponent<Weapons>();

        
        if (weapons != null)
        {
            weapons.OnWeaponChange += HandleWeaponChange; 
        }
    }

    // Método que maneja el evento OnWeaponChange
    private void HandleWeaponChange(Weapons.WeaponState newWeaponState)
    {
       
        Debug.Log("El arma ha cambiado a: " + newWeaponState.ToString());

        
    }

    void OnDestroy()
    {
        // Asegurarse de desuscribirse del evento cuando el objeto se destruya
        if (weapons != null)
        {
            weapons.OnWeaponChange -= HandleWeaponChange;
        }
    }
}
