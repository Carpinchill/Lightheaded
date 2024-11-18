using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generador : MonoBehaviour
{
    public float activationDistance = 2f; //distancia para activar el generador
    private bool _isActive = false; //para comprobar si el generador est� activo
    private Transform playerTransform;
    public GameObject sign;

    void Start()
    {
        //buscar el jugador por su etiqueta
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        sign.SetActive(true);
    }

    void Update()
    {
        //comprobar si el jugador est� cerca
        if (Vector3.Distance(transform.position, playerTransform.position) < activationDistance && !_isActive)
        {
            //comprobar si se presiona la tecla "E"
            if (Input.GetKey(KeyCode.E))
            {
                StartCoroutine(ActivateGenerator());
            }
        }
    }

    private IEnumerator ActivateGenerator()
    {
        //esperar 4 segundos mientras se mantiene "E"
        float holdTime = 0f;

        while (Input.GetKey(KeyCode.E) && holdTime < 4f)
        {
            holdTime += Time.deltaTime;
            yield return null;
        }

        //comprobar si se mantuvo durante 4 segundos
        if (holdTime >= 4f)
        {
            _isActive = true;
            Debug.Log("Se activ� el generador");
            sign.SetActive(false);
        }
    }
}
