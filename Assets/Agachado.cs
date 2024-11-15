using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agachado : MonoBehaviour
{
    private Animator animator;
    private bool isCrouching = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.E))
        {
            isCrouching = true;
            animator.SetBool("isCrouching", true);
        }
        else
        {
            isCrouching = false;
            animator.SetBool("isCrouching", false);
        }
    }
}
