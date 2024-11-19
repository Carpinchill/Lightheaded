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

    // Método que maneja el evento OnWeaponChange
    private void HandleWeaponChange(Weapons.WeaponState newWeaponState)
    {
        // Imprimir el nuevo estado del arma en la consola
        Debug.Log("El arma ha cambiado a: " + newWeaponState.ToString());

        // Aquí podrías colocar código para activar una animación cuando tengas las animaciones
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
