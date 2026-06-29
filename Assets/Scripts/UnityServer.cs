using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

[Serializable]
public class TankMessage
{
    public float turretYaw;
    public float gunPitch;
    public float posX;
    public float posZ;
    public float bodyRotation;

    public bool fire;
}

public class UnityServer : MonoBehaviour
{
    public int port = 6001;

    public float turretYaw;
    public float gunPitch;
    public float posX;
    public float posZ;
    public float bodyRotation;
    public bool fire;

    public float turretYaw2P;
    public float gunPitch2P;
    public float posX2P;
    public float posZ2P;
    public float bodyRotation2P;
    public bool fire2P;

    TcpListener listener;
    TcpClient client;

    StreamReader reader;
    StreamWriter writer;

    Thread receiveThread;
    Thread sendThread;

    private bool shuttingDown = false;

    void Start()
    {
        Thread serverThread = new Thread(ServerLoop);
        serverThread.IsBackground = true;
        serverThread.Start();
    }

    void ServerLoop()
    {
        try
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            Debug.Log("Server Waiting...");

            client = listener.AcceptTcpClient();

            if (shuttingDown)
                return;

            Debug.Log("Client Connected!");

            NetworkStream stream = client.GetStream();

            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            receiveThread = new Thread(ReceiveLoop);
            receiveThread.IsBackground = true;
            receiveThread.Start();

            sendThread = new Thread(SendLoop);
            sendThread.IsBackground = true;
            sendThread.Start();
        }
        catch (SocketException)
        {
            if (!shuttingDown)
                Debug.LogError("Accept Error");
        }
        catch (Exception e)
        {
            if (!shuttingDown)
                Debug.LogError(e);
        }
    }

    void ReceiveLoop()
    {
        try
        {
            while (!shuttingDown)
            {
                string msg = reader.ReadLine();

                if (!string.IsNullOrEmpty(msg))
                {
                    TankMessage data = JsonUtility.FromJson<TankMessage>(msg);

                    //Debug.Log($"Turret : {data.turretYaw}");
                    //Debug.Log($"Gun : {data.gunPitch}");
                    //Debug.Log($"Fire : {data.fire}");
                    turretYaw2P = data.turretYaw;
                    gunPitch2P = data.gunPitch;
                    posX2P = data.posX;
                    posZ2P = data.posZ;
                    bodyRotation2P = data.bodyRotation;
                    if (data.fire)
                    {
                        fire2P = true;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Receive End : " + e.Message);
        }
    }

    void SendLoop()
    {
        int count = 0;

        try
        {
            while (!shuttingDown)
            {
                TankMessage msg = new TankMessage();

                msg.turretYaw = turretYaw;
                msg.gunPitch = gunPitch;
                msg.posX = posX;
                msg.posZ = posZ;
                msg.bodyRotation = bodyRotation;
                msg.fire = fire;

                string json = JsonUtility.ToJson(msg);

                writer.WriteLine(json);
                fire = false;
                count++;
                Thread.Sleep(10);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Send End : " + e.Message);
        }
    }

    void CloseServer()
    {
        shuttingDown = true;

        try { writer?.Close(); } catch { }
        try { reader?.Close(); } catch { }
        try { client?.Close(); } catch { }
        try { listener?.Stop(); } catch { }
    }

    void OnDestroy()
    {
        CloseServer();
    }

    void OnApplicationQuit()
    {
        CloseServer();
    }
}