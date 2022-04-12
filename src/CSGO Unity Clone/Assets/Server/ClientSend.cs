using Assets.Server;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTcpData(Packet packet) 
    {
        packet.WriteLength();
        Client.Instance.TcpClient.SendData(packet);
    }

    #region Packets
    public static void WelcomeReceived() 
    {
        using (Packet packet = new Packet((int)ClientPackets.welcomeReceived)) 
        {
            packet.Write(Client.Instance.ClientId);
            packet.Write(UIManager.Instance.UsernameField.text);
            
            SendTcpData(packet);
        }
    }
    #endregion
}
