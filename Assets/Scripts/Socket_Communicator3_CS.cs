using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Security.Cryptography.X509Certificates;

[Serializable]
public class VoiceResult
{
    public string command;
}

[Serializable]
public class VoicePacket
{
    public string role;
    public string device_id;
    public int frame_id;
    public string timestamp;
    public float fps;
    public VoiceResult result;
}

public class Socket_Communicator3_CS : MonoBehaviour
{
    [Header("Network")]
    public int port = 5014;

    public string command = "none";

    private TcpListener listener;
    private TcpClient client;
    private Thread serverThread;

    private readonly object dataLock = new object();

    // 최신 수신 데이터
    private VoiceResult latestResult;
    private bool newDataReceived = false;

    private bool shuttingDown = false;

    public bool scanning = false;
    public bool reloading = false;

    void Start()
    {
        serverThread = new Thread(ServerLoop);
        serverThread.IsBackground = true;
        serverThread.Start();
    }

    private void ServerLoop()
    {
        try
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            Debug.Log($"TCP Server Start : {port}");

            client = listener.AcceptTcpClient();

            Debug.Log("Pi Connected");

            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            byte[] buffer = new byte[4096];

            while (!shuttingDown)
            {
                string json = reader.ReadLine();

                if (string.IsNullOrWhiteSpace(json))
                    continue;

                try
                {
                    VoicePacket packet =
                        JsonUtility.FromJson<VoicePacket>(json);

                    //Debug.Log($"RAW JSON = {json}");

                    lock (dataLock)
                    {
                        latestResult = packet.result;
                        newDataReceived = true;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(
                        $"JSON Parse Fail\n{ex.Message}\n{json}");
                }
            }
        }
        catch (Exception e)
        {
            if (!shuttingDown)
            {
                Debug.LogError($"Server Error : {e.Message}");
            }
        }
    }

    void Update()
    {
        if (!newDataReceived)
            return;

        VoiceResult data;

        lock (dataLock)
        {
            data = latestResult;
            newDataReceived = false;
        }

        // Unity 메인 스레드에서 안전하게 사용
        //Debug.Log(
        //    $"Pose:{data.has_pose} " +
        //    $"Yaw:{data.yaw_value:F2}" +
        //    $"Pitch:{data.pitch_value:F2}" +
        //    $"Fire:{data.fire}");

        // 예시: 탱크 제어

        command = data.command;

        if(command == "scanning")
            scanning = true;
        else if (command == "reload")
            reloading = true;

        // TankController.SetTrackInput(leftTrack, rightTrack);
    }

    void CloseSocket()
    {
        shuttingDown = true;

        try { client?.Close(); } catch { }
        try { listener?.Stop(); } catch { }

        if (serverThread != null && serverThread.IsAlive)
            serverThread.Join(200);
    }

    void OnDestroy()
    {
        CloseSocket();
    }

    void OnApplicationQuit()
    {
        CloseSocket();
    }
}