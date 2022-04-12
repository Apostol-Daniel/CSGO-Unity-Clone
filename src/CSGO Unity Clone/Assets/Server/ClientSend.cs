using Assets.Server;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTcpData(Packet packet) 
    {
        packet.WriteLength();
        Client.ClientInstance.TcpClient.SendData(packet);
    }

    private static void SendUdpData(Packet packet) 
    {
        packet.WriteLength();
        Client.ClientInstance.UdpClient.SendData(packet);
    }

    #region Packets
    public static void WelcomeReceived() 
    {
        using (Packet packet = new Packet((int)ClientPackets.welcomeReceived)) 
        {
            packet.Write(Client.ClientInstance.ClientId);
            packet.Write(UIManager.Instance.UsernameField.text);
            
            SendTcpData(packet);
        }
    }

    public static void UdpTestReceived() 
    {
        using (Packet packet = new Packet((int)ClientPackets.udpTestReceived)) 
        {
            packet.Write("Received a UDP packet");

            SendUdpData(packet);
        }
    }
    #endregion
}
