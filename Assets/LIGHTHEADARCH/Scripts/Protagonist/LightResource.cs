using System.Collections;
using UnityEngine;

public class LightResource : MonoBehaviour
{
    //BRUNO
    private bool _isRegenerating = false;
    public float regenRate = 0.1f; 

    public float maxShine = 100f; 
    public float intensityMax = 2.0f; 
    private float _ResidualLight = 0.1f; 

    [SerializeField]
    private float _currentShine; 

    public Light lampLight; 

    void Start()
    {
        if (lampLight == null)
        {
            lampLight = GetComponentInChildren<Light>(); 
        }

        _currentShine = maxShine; 
        UpdateLightIntensity(); 
    }

    void Update()
    {
        
        UpdateLightIntensity();
    }

    
    public void StartRegen()
    {
        if (!_isRegenerating && _currentShine < maxShine)
        {
            _isRegenerating = true;
            StartCoroutine(RegenerateShine());
        }
    }

    
    public void StopRegen()
    {
        if (_isRegenerating)
        {
            _isRegenerating = false;
            StopCoroutine(RegenerateShine());
        }
    }

    
    private IEnumerator RegenerateShine()
    {
        while (_currentShine < maxShine && _isRegenerating)
        {
            _currentShine += regenRate * Time.deltaTime;
            _currentShine = Mathf.Clamp(_currentShine, 0f, maxShine); 
            yield return null;
        }
    }

    
    public void RechargeShine(float amount)
    {
        _currentShine += amount;
        _currentShine = Mathf.Clamp(_currentShine, 0f, maxShine); 
        UpdateLightIntensity();
    }

    
    public void UseShine(float amount)
    {
        _currentShine -= amount;
        _currentShine = Mathf.Clamp(_currentShine, 0f, maxShine); 
        UpdateLightIntensity();
    }

    
    private void UpdateLightIntensity()
    {
        if (lampLight != null)
        {
            
            lampLight.intensity = intensityMax * ((_currentShine + _ResidualLight) / (maxShine + _ResidualLight));
        }
    }

    
    public float GetCurrentShine()
    {
        return _currentShine;
    }
}
