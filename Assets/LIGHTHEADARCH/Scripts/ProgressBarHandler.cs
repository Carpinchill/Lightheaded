using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ProgressBarHandler : MonoBehaviour
{
    public Image progressBar;

    private float progress = 0f;

    public void UpdateProgress(float increment)
    {
        progress += increment;
        progress = Mathf.Clamp01(progress); // Asegura que el progreso esté entre 0 y 1
        if (progressBar != null)
        {
            progressBar.fillAmount = progress;
        }
    }

    public bool IsComplete() => progress >= 1f;

    public void ResetProgress()
    {
        progress = 0f;
        if (progressBar != null)
        {
            progressBar.fillAmount = 0f;
        }
    }

    public void ShowProgressBar(bool show)
    {
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(show);
        }
    }
}