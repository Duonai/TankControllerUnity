using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

[Serializable]
public class ClientMessage
{
    public float turretYaw;
    public float gunPitch;
    public float posX;
    public float posZ;
    public float bodyRotation;

    public bool fire;

    public float HP;
    public float HP2;
}

public class UnityClient : MonoBehaviour
{
    public string serverIP = "192.168.0.102";
    public int port = 6001;

    public float turretYaw;
    public float gunPitch;
    public float posX;
    public float posZ;
    public float bodyRotation;
    public bool fire;
    public float HP = 600f;
    public float HP2 = 600f;

    public float turretYaw1P;
    public float gunPitch1P;
    public float posX1P;
    public float posZ1P;
    public float bodyRotation1P;
    public bool fire1P;
    public float HP1P = 600f;
    public float HP1P2 = 600f;

    TcpClient client;

    StreamReader reader;
    StreamWriter writer;

    Thread receiveThread;
    Thread sendThread;

    private bool shuttingDown = false;

    void Start()
    {
        client = new TcpClient();

        client.Connect(serverIP, port);

        Debug.Log("Connected");

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

    void ReceiveLoop()
    {
        try
        {
            while (!shuttingDown)
            {
                string msg = reader.ReadLine();

                if (!string.IsNullOrEmpty(msg))
                {
                    ClientMessage data = JsonUtility.FromJson<ClientMessage>(msg);

                    //Debug.Log($"Turret : {data.turretYaw}");
                    //Debug.Log($"Gun : {data.gunPitch}");
                    Debug.Log($"Fire : {data.fire}");

                    turretYaw1P = data.turretYaw;
                    gunPitch1P = data.gunPitch;
                    posX1P = data.posX;
                    posZ1P = data.posZ;
                    bodyRotation1P = data.bodyRotation;
                    if (data.fire)
                    {
                        fire1P = true;
                    }
                    HP1P = data.HP;
                    HP1P2 = data.HP2;
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
                ClientMessage msg = new ClientMessage();

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
                Thread.Sleep(5);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Send End : " + e.Message);
        }
    }

    void CloseSocket()
    {
        shuttingDown = true;

        try { client?.Close(); } catch { }
        try { reader?.Close(); } catch { }
        try { writer?.Close(); } catch { }

        if (receiveThread != null && receiveThread.IsAlive)
            receiveThread.Join(100);

        if (sendThread != null && sendThread.IsAlive)
            sendThread.Join(100);
    }

    void OnApplicationQuit()
    {
        CloseSocket();
    }

    void OnDestroy()
    {
        CloseSocket();
    }
}