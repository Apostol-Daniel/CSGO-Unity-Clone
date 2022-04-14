using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Id;
    public string UserName;
    public CharacterController CharacterController;

    public float MoveSpeed = 5f;
    public float Gravity = -19.62f;
    public float JumpSpeed = 5;

    public float YVelocity = 0;
    private bool[] Inputs;

    public void Start()
    {
        Gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        MoveSpeed *= Time.fixedDeltaTime;
        JumpSpeed *= Time.fixedDeltaTime;
    }
    public void Initialize(int id, string userName)
    {
        Id = id;
        UserName = userName;      
        Inputs = new bool[5];
    }

    public void FixedUpdate()
    {
        Vector2 inputDirection = Vector2.zero;

        if (Inputs[0])
        {
            inputDirection.y += 1;
        }
        if (Inputs[1])
        {
            inputDirection.y -= 1;
        }
        if (Inputs[2])
        {
            inputDirection.x -= 1;
        }
        if (Inputs[3])
        {
            inputDirection.x += 1;
        }

        Move(inputDirection);
    }

    private void Move(Vector2 inputDirection)
    {       
        Vector3 moveDirection = transform.right * inputDirection.x + transform.forward * inputDirection.y;
        moveDirection *= MoveSpeed;

        if (CharacterController.isGrounded) 
        {
            YVelocity = 0f;
            if (Inputs[4]) 
            {
                YVelocity = JumpSpeed;
            }
        }

        YVelocity += Gravity;

        moveDirection.y = YVelocity;
        CharacterController.Move(moveDirection);

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    public void SetInput(bool[] inputs, Quaternion rotation)
    {
        Inputs = inputs;
        transform.rotation = rotation;
    }
}
