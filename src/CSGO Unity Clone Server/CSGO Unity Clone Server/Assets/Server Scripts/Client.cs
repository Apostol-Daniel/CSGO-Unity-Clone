using Assets.Server_Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client
{
    public int Id;
    public Tcp Tcp;
    public Udp Udp;
    public Player Player;
    //4 MB's
    public static int DataBufferSize = 4096;

    public Client(int clientId)
    {
        Id = clientId;
        Tcp = new Tcp(Id, DataBufferSize);
        Udp = new Udp(Id);
    }

    public void SendIntoGame(string playerName)
    {
        Player = NetworkManager.Instance.InstantiatePlayer();
        Player.Initialize(Id, playerName);

        //Send alreday connected players info to new player
        foreach (Client client in Server.Clients.Values)
        {
            if (client.Player != null)
            {
                if (client.Id != Id)
                {
                    ServerSend.SpawnPlayer(Id, client.Player);
                }
            }
        }

        //Send new player info to all players and himself
        foreach (Client client in Server.Clients.Values)
        {
            if (client.Player != null)
            {
                ServerSend.SpawnPlayer(client.Id, Player);
            }
        }

        foreach(ItemSpawner spawner in ItemSpawner.Spawners.Values) 
        {
            if(spawner!= null) 
            {
                ServerSend.CreateItemSpawner(Id, spawner.SpawnerId, spawner.transform.position, spawner.HasItem);
            }
        }

        foreach(Enemy enemy in Enemy.Enemies.Values) 
        {
            ServerSend.SpawnEnemiesForNewClient(Id, enemy);
        }
    }

    public void Disconnect()
    {
        Debug.Log($"{Tcp.Socket.Client.RemoteEndPoint} has disconnected.");

        ThreadManager.ExecuteOnMainThread(() =>
        {
            UnityEngine.Object.Destroy(Player.gameObject);
            Player = null;
        });

        Tcp.Disconnect();
        Udp.Disconnect();

        ServerSend.PlayerDisconnected(Id);
    }
}
