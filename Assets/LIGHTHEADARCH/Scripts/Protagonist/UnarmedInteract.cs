using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnarmedInteract : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rayDistance = 5f;
    public Weapons weapons;
    public CameraManager cameraManager;

    void Update()
    {
        if (weapons.currentWeapon == Weapons.WeaponState.Unarmed && cameraManager.isAiming && Input.GetMouseButtonDown(0))
        {
            FireLightShard();
        }
    }

    //m�todo para disparar un raycast desde el centro de la pantalla
    private void FireLightShard()
    {
        //crear un ray desde el centro de la pantalla
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        //verificar si el raycast golpea algo
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            //intentar obtener el script ParedController
            if (hit.collider.TryGetComponent<FlameInteractuable>(out var pared))
            {
                pared.Desaparecer(); //llama al m�todo de la pared para hacerla desaparecer
            }
            else
            {
                Debug.Log("Golpeaste a: " + hit.collider.name);
            }
        }
    }
}
