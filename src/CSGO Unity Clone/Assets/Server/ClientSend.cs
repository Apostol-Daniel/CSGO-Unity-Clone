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
        using (Packet packet = new Packet((int)ClientPackets.WelcomeReceived)) 
        {
            packet.Write(Client.ClientInstance.ClientId);
            packet.Write(UIManager.Instance.UsernameField.text);
            
            SendTcpData(packet);
        }
    }   

    public static void PlayerMovement(bool[] inputs) 
    {
        using(Packet packet = new Packet((int)ClientPackets.PlayerMovement)) 
        {
            packet.Write(inputs.Length);
            foreach (bool input in inputs)
            {
                packet.Write(input);
            }

            packet.Write(GameManager.Players[Client.ClientInstance.ClientId].transform.rotation);

            SendUdpData(packet);
        }
    }
    #endregion
}
