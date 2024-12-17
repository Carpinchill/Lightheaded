using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Light lightToDeactivate;
    public Light lightToActivate;

    public void ToggleLights()
    {
        if (lightToDeactivate != null)
        {
            lightToDeactivate.enabled = false;
        }
        if (lightToActivate != null)
        {
            lightToActivate.enabled = true;
        }
    }
}
