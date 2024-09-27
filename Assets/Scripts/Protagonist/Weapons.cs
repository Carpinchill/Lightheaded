using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public enum WeaponState { Unarmed, Flashlight, FutureWeapon }
    public WeaponState currentWeapon = WeaponState.Unarmed;

    public GameObject unarmed;
    public GameObject flashlight; // Referencia a la linterna
    public GameObject futureWeapon; // Referencia para la futura arma

    void Update()
    {
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

    void EquipWeapon()
    {
        flashlight.SetActive(currentWeapon == WeaponState.Flashlight);
        futureWeapon.SetActive(currentWeapon == WeaponState.FutureWeapon);
        // Aquí podrías poner la lógica para "desarmado"
    }
}
