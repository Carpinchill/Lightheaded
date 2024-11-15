using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textoProgreso;
    [SerializeField] Slider sliderProgreso;

    private void Update()
    {
        if(Input.anyKey)
        {
            StartCoroutine(Carga());
        }
    }

    IEnumerator Carga()
    {
        sliderProgreso.gameObject.SetActive(true);
        AsyncOperation operacionCarga = SceneManager.LoadSceneAsync(2);

        while (operacionCarga.isDone == false)
        {
            float progreso = Mathf.Clamp01(operacionCarga.progress / 0.9f);
            sliderProgreso.value = progreso;
            textoProgreso.text = "" + progreso * 100 + "%";
            yield return null;
        }
        
    }
}
