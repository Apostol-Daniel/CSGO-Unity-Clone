using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Tcp
{
    public TcpClient Socket;

    private readonly int Id;
    private readonly int DataBufferSize;
    private NetworkStream Stream;
    private byte[] ReceiveBuffer;
    private Packet ReceivedData;

    public Tcp(int id, int dataBufferSize)
    {
        Id = id;
        DataBufferSize = dataBufferSize;
    }

    public void Connect(TcpClient socket)
    {
        Socket = socket;
        Socket.ReceiveBufferSize = DataBufferSize;
        Socket.SendBufferSize = DataBufferSize;

        Stream = Socket.GetStream();

        ReceivedData = new Packet();
        ReceiveBuffer = new byte[DataBufferSize];

        Stream.BeginRead(ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);

        ServerSend.Welcome(Id, "Welcome to the server!");
    }

    public void SendData(Packet packet)
    {
        try
        {
            if (Socket != null)
            {
                Stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
            }
        }
        catch (Exception ex)
        {

            Debug.Log($"Error sending data to player {Id} via TCP: {ex}.");
        }
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            int byteLenght = Stream.EndRead(result);
            if (byteLenght <= 0)
            {
                Server.Clients[Id].Disconnect();
                return;
            }

            byte[] data = new byte[byteLenght];
            Array.Copy(ReceiveBuffer, data, byteLenght);

            ReceivedData.Reset(HandleData(data));
            Stream.BeginRead(ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
        }
        catch (Exception ex)
        {
            Debug.Log($"Error receiving TCP data : {ex}");
            Server.Clients[Id].Disconnect();
        }
    }

    private bool HandleData(byte[] data)
    {
        int packetLenght = 0;

        ReceivedData.SetBytes(data);

        if (ReceivedData.UnreadLength() >= 4)
        {
            packetLenght = ReceivedData.ReadInt();
            if (packetLenght <= 0)
            {
                return true;
            }
        }

        while (packetLenght > 0 && packetLenght <= ReceivedData.UnreadLength())
        {
            byte[] packetBytes = ReceivedData.ReadBytes(packetLenght);
            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet packet = new Packet(packetBytes))
                {
                    int packetId = packet.ReadInt();
                    Server.PacketHandlers[packetId](Id, packet);
                }
            });

            packetLenght = 0;
            if (ReceivedData.UnreadLength() >= 4)
            {
                packetLenght = ReceivedData.ReadInt();
                if (packetLenght <= 0)
                {
                    return true;
                }
            }
        }

        if (packetLenght <= 1)
        {
            return true;
        }

        return false;
    }

    public void Disconnect()
    {
        Socket.Close();
        Stream = null;
        ReceivedData = null;
        ReceiveBuffer = null;
        Socket = null;
    }
}
