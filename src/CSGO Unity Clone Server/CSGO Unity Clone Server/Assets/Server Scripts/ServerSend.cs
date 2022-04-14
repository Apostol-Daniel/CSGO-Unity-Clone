using System.Collections;
using System.Collections.Generic;

public class ServerSend
{
    #region Packets
    public static void Welcome(int toClient, string message)
    {
        using (Packet packet = new Packet((int)ServerPackets.Welcome))
        {
            packet.Write(message);
            packet.Write(toClient);

            SendTcpData(toClient, packet);
        }
    }

    public static void SpawnPlayer(int clientId, Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.SpawnPlayer))
        {
            packet.Write(player.Id);
            packet.Write(player.UserName);
            packet.Write(player.transform.position);
            packet.Write(player.transform.rotation);
            SendTcpData(clientId, packet);
        }

    }

    public static void PlayerPosition(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.PlayerPosition))
        {
            packet.Write(player.Id);
            packet.Write(player.transform.position);

            SendUdpDataToAll(packet);
        }
    }

    public static void PlayerRotation(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.PlayerRotation))
        {
            packet.Write(player.Id);
            packet.Write(player.transform.rotation);

            SendUdpDataToAllExceptOneClient(player.Id, packet);
        }
    }

    public static void PlayerDisconnected(int playedId) 
    {
        using(Packet packet = new Packet((int)ServerPackets.PlayerDisconnected)) 
        {
            packet.Write(playedId);

            SendTcpDataToAll(packet);
        }
    }

    public void PlayerHealth(Player player) 
    {
        using(Packet packet = new Packet((int)ServerPackets.PlayerHealth)) 
        {
            packet.Write(player.Id);
            packet.Write(player.Health);

            SendTcpDataToAll(packet);
        }
    }

    public static void PlayerRespawed(Player player) 
    {
        using(Packet packet = new Packet((int)ServerPackets.PlayerRespawned)) 
        {
            packet.Write(player.Id);

            SendTcpDataToAll(packet);
        }
    }
    #endregion

    #region UdpData
    private static void SendUdpData(int clientId, Packet packet)
    {
        packet.WriteLength();
        Server.Clients[clientId].Udp.SendData(packet);
    }

    public static void SendUdpDataToAll(Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.Clients[i].Udp.SendData(packet);
        }
    }

    public static void SendUdpDataToAllExceptOneClient(int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != exceptClient)
            {
                Server.Clients[i].Udp.SendData(packet);
            }
        }
    }

    #endregion

    #region TcpData

    private static void SendTcpData(int clientId, Packet packet)
    {
        packet.WriteLength();
        Server.Clients[clientId].Tcp.SendData(packet);
    }

    public static void SendTcpDataToAll(Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.Clients[i].Tcp.SendData(packet);
        }
    }

    public static void SendTcpDataToAllExceptOneClient(int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != exceptClient)
            {
                Server.Clients[i].Tcp.SendData(packet);
            }
        }
    }

    #endregion
}
