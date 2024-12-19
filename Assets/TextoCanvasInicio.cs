using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextoCanvasInicio : MonoBehaviour
{
    //FRANCO
    public TextMeshProUGUI objectiveText;
    public float displayDuration = 5f;

    void Start()
    {

        if (objectiveText != null)
        {
            objectiveText.gameObject.SetActive(true);
            Invoke("HideObjectiveText", displayDuration);
        }
    }

    void HideObjectiveText()
    {
        if (objectiveText != null)
        {
            objectiveText.gameObject.SetActive(false);
        }
    }

}
