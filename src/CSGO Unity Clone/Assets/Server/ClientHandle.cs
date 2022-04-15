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

        public static void PlayerHealth(Packet packet) 
        {
            int id = packet.ReadInt();
            float health = packet.ReadFloat();

            GameManager.Players[id].SetHealth(health);
        }

        public static void PlayerRespwaned(Packet packet) 
        {
            int id = packet.ReadInt();

            GameManager.Players[id].Respawn();
        }

        public static void CreateItemSpawner(Packet packet) 
        {
            int spawnerId = packet.ReadInt();
            Vector3 spawnPosition = packet.ReadVector3();
            bool hasItem = packet.ReadBool();

            GameManager.Instance.CreateItemSpawner(spawnerId, spawnPosition, hasItem);
        }

        public static void ItemSpawned(Packet packet)
        {
            int spawnerId = packet.ReadInt();

            GameManager.Spawners[spawnerId].ItemSpawned();
        }

        public static void ItemPickedUp(Packet packet)
        {
            int spawnerId = packet.ReadInt();
            int playerId = packet.ReadInt();

            GameManager.Spawners[spawnerId].ItemPickedUp();
            GameManager.Players[playerId].ItemCount++;
        }

        public static void SpawnProjectile(Packet packet) 
        {
            int projectleId = packet.ReadInt();
            Vector3 position = packet.ReadVector3();
            int playerId = packet.ReadInt();

            GameManager.Instance.SpawnProjcetile(projectleId, position);
            GameManager.Players[playerId].ItemCount--;
        }

        public static void ProjectilePosition(Packet packet)
        {
            int projectileId = packet.ReadInt();
            Vector3 position = packet.ReadVector3();

            GameManager.Projectiles[projectileId].transform.position = position;
        }

        public static void ProjectileExploded(Packet packet)
        {
            int projectileId = packet.ReadInt();
            Vector3 position = packet.ReadVector3();

            GameManager.Projectiles[projectileId].Explode(position);
        }

    }


}
