using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento_Menu : MonoBehaviour
{
    float mousePosX;
    float mousePosY;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePosX = Input.mousePosition.x;
        mousePosY = Input.mousePosition.y;

        this.GetComponent<RectTransform>().position = new Vector2((mousePosX / Screen.width) * 20 + (Screen.width / 2), (mousePosY / Screen.height) * 20 + (Screen.height / 2));


    }
}
