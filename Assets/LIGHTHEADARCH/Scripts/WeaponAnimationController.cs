using UnityEngine;

public class WeaponAnimationController : MonoBehaviour
{
    private Weapons weapons;  // Referencia al script Weapons

    void Start()
    {
        // Obtener la referencia al script Weapons
        weapons = GetComponent<Weapons>();

        // Suscribirse al evento OnWeaponChange
        if (weapons != null)
        {
            weapons.OnWeaponChange += HandleWeaponChange; // Suscribirse al evento
        }
    }

    // M�todo que maneja el evento OnWeaponChange
    private void HandleWeaponChange(Weapons.WeaponState newWeaponState)
    {
        // Imprimir el nuevo estado del arma en la consola
        Debug.Log("El arma ha cambiado a: " + newWeaponState.ToString());

        // Aqu� podr�as colocar c�digo para activar una animaci�n cuando tengas las animaciones
        // Ejemplo: animator.SetTrigger("ChangeWeapon");
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
