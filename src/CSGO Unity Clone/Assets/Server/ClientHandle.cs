using System.Collections;
using System.Collections.Generic;
using System.Net;
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

            Client.ClientInstance.ClientId = clientId;

            ClientSend.WelcomeReceived();

            Client.ClientInstance.UdpClient.Connect(((IPEndPoint)Client.ClientInstance.TcpClient.TcpSocket.Client.LocalEndPoint).Port);
        }

        public static void SpawnPlayer(Packet packet) 
        {
            int playerId = packet.ReadInt();
            string playerUserName = packet.ReadString();
            Vector3 playerPosition = packet.ReadVector3();
            Quaternion playerRotation = packet.ReadQuaternion();

            GameManager.Instance.SpawnPlayer(playerId, playerUserName, playerPosition, playerRotation);
        }
    }


}
