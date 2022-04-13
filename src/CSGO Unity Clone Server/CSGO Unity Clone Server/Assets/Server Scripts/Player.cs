using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Id;
    public string UserName; 

    private float MoveSpeed = 5f / ServerTicks.TicksPerSecond;
    private bool[] Inputs;

    public void Initialize(int id, string userName)
    {
        Id = id;
        UserName = userName;      
        Inputs = new bool[4];
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
        transform.position += moveDirection * MoveSpeed;

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    public void SetInput(bool[] inputs, Quaternion rotation)
    {
        Inputs = inputs;
        transform.rotation = rotation;
    }
}
