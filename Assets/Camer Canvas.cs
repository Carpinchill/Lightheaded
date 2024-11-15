using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerCanvas : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Camera.main.transform.rotation, 100f);
    }
}
