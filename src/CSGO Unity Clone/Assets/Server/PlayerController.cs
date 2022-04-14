using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform CameraTransform;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            ClientSend.PlayerShoot(CameraTransform.forward);
        }
    }

    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void SendInputToServer() 
    {
        bool[] inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetButtonDown("Jump"),
        };

        ClientSend.PlayerMovement(inputs);
    }
}
