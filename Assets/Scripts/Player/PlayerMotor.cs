using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float speed = 5f;

    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    public bool crouching = false;
    public float crouchTimer = 1;
    public bool lerpCrouch = false;
    public bool sprinting = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;

            if (crouching)
            {
                controller.height = Mathf.Lerp(controller.height, 1, p);
            }
            else
            {
                controller.height = Mathf.Lerp(controller.height, 2, p);
            }

            if (p >= 1)
            {
                lerpCrouch = false;
            }
        }

    }
    //receive input from our InuputManager.cs script and apply them to our character contoller 
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * Time.deltaTime * speed);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }
    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
    public void Crouch()
    {
        if (sprinting)
        {
            sprinting = !sprinting;

        }
        speed = 2f;
        crouching = !crouching;
        crouchTimer = 0f;
        lerpCrouch = true;

    }
    public void Sprint()
    {
        if (crouching) return;
        sprinting = !sprinting;
        if (sprinting)
        {
            speed = 20f;
        }
        else
        {
            speed = 5f;
        }
    }
}
