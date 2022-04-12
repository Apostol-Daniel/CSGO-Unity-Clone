using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Server 
{
    public class ClientHandle : MonoBehaviour
    {
        public static void Welcome(Packet packet) 
        {
            string message = packet.ReadString();
            int clientId = packet.ReadInt();

            Debug.Log($"Message from server: {message}");

            Client.Instance.ClientId = clientId;

            ClientSend.WelcomeReceived();
        }
    }

}
