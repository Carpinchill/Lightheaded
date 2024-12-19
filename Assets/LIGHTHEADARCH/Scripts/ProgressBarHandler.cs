using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ProgressBarHandler : MonoBehaviour
{
    //FRANCO
    public Image progressBar;

    private float _progress = 0f;

    public void UpdateProgress(float increment)
    {
        _progress += increment;
        _progress = Mathf.Clamp01(_progress); 
        if (progressBar != null)
        {
            progressBar.fillAmount = _progress;
        }
    }

    public bool IsComplete() => _progress >= 1f;

    public void ResetProgress()
    {
        _progress = 0f;
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