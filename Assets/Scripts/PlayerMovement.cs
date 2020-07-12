using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;

    public float runSpeed = 40f;

    private float horizontalMove = 0f;
    private bool jump = false;
    private bool dash = false;
    
    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isPaused) return;

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        else if (Input.GetButtonDown("Dash"))
        {
            dash = true;
        }

    }

    void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, dash, jump);
        jump = false;
        dash = false;
    }
}