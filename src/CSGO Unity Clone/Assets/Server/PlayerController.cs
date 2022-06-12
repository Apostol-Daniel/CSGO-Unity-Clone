using Assets.Server;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform CameraTransform;

    void Update()
    {
        EscapeToMainMenu();

        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            Debug.Log("PlayerController Shoot");
            ClientSend.PlayerShoot(CameraTransform.forward);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("PlayerController Throw");
            ClientSend.PlayerThrowItem(CameraTransform.forward);
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
            Input.GetKey(KeyCode.Space),
        };

        ClientSend.PlayerMovement(inputs);
    }

    public void EscapeToMainMenu()
    {
        //Getting Escape(Esc) button input
        if (Input.GetButtonDown("Cancel"))
        {
            UIManager.Instance().EscapeToMainMenuSingleMultiplayer();
        }
    }
}
