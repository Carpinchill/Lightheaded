using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rayDistance = 5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Al hacer clic izquierdo
        {
            FireLightShard();
        }
    }

    // Método para disparar un raycast desde el centro de la pantalla
    private void FireLightShard()
    {
        // Crear un ray desde el centro de la pantalla
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // Verificar si el raycast golpea algo
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            // Intentar obtener el script ParedController
            if (hit.collider.TryGetComponent<FlameInteractuable>(out var pared))
            {
                pared.Desaparecer(); // Llama al método de la pared para hacerla desaparecer
            }
        }
    }
}

