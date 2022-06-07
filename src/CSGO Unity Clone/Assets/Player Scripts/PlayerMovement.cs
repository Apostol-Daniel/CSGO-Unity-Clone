using Assets.Server;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController CharacterController;

    public float Speed = 5f;
    public float Gravity = -19.62f;
    public float JumpHeight = 2f;

    Vector3 Velocity;
    bool IsGrounded;

    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;
    
    // Update is called once per frame
    void Update()
    {
        EscapeToMainMenu();

        IsGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance,GroundMask);
        if(IsGrounded && Velocity.y < 0) 
        {
            Velocity.y = -2f;
        }

        #region Moving around
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        CharacterController.Move(move * Speed * Time.deltaTime);
        #endregion
        if(Input.GetButtonDown("Jump")&& IsGrounded)
        {
            Velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
        }

        Velocity.y += Gravity * Time.deltaTime;

        CharacterController.Move(Velocity * Time.deltaTime);
    }

    public void EscapeToMainMenu()
    {
        //Getting Escape(Esc) button input
        if (Input.GetButtonDown("Cancel"))
        {
            UIManager.Instance().EscapeToMainMenu();
        }
    }

}
