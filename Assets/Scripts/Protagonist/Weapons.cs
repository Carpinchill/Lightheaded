using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public enum WeaponState { Unarmed, Flashlight, FutureWeapon }
    public WeaponState currentWeapon = WeaponState.Unarmed;
    public GameObject unarmed;
    public GameObject flashlight; // referencia a la linterna
    public GameObject futureWeapon; // referencia para la futura arma

    private PlayerMovement playerMovement; // Referencia al PlayerMovement
    public FlashLight flashLightMan; // Referencia al controlador de la linterna

    void Start()
    {
        flashlight.SetActive(false);
        playerMovement = GetComponent<PlayerMovement>(); // Obtener la referencia al PlayerMovement
    }

    void Update()
    {
        // Cambiar el arma seg�n la tecla presionada
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = WeaponState.Unarmed;
            EquipWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = WeaponState.Flashlight;
            EquipWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeapon = WeaponState.FutureWeapon;
            EquipWeapon();
        }
    }

    // M�todo para activar el arma seg�n el estado
    void EquipWeapon()
    {
        flashlight.SetActive(currentWeapon == WeaponState.Flashlight);
        futureWeapon.SetActive(currentWeapon == WeaponState.FutureWeapon);
        unarmed.SetActive(currentWeapon == WeaponState.Unarmed);
    }
}