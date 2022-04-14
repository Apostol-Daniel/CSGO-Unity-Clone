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

        public static void PlayerPosition(Packet packet) 
        {
            int playerId = packet.ReadInt();
            Vector3 postition = packet.ReadVector3();

            GameManager.Players[playerId].transform.position = postition;
        }

        public static void PlayerRotation(Packet packet)
        {
            int playerId = packet.ReadInt();
            Quaternion rotation = packet.ReadQuaternion();

            GameManager.Players[playerId].transform.rotation = rotation;
        }

        public static void PlayerDisconnected(Packet packet) 
        {
            int playerId = packet.ReadInt();

            Destroy(GameManager.Players[playerId].gameObject);
            GameManager.Players.Remove(playerId);
        }
    }


}
