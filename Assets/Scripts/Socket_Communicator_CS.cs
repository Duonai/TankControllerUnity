using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;

[Serializable]
public class TrackResult
{
    public bool has_pose;
    public float left_value;
    public float right_value;
    public string left_label;
    public string right_label;
    public string drive_label;
}

[Serializable]
public class TrackPacket
{
    public string role;
    public string device_id;
    public int frame_id;
    public string timestamp;
    public float fps;
    public TrackResult result;
}

public class Socket_Communicator_CS : MonoBehaviour
{
    [Header("Network")]
    public int port = 5012;

    public float leftTrack = 0.0f;
    public float rightTrack = 0.0f;

    private TcpListener listener;
    private TcpClient client;
    private Thread serverThread;

    private readonly object dataLock = new object();

    // 최신 수신 데이터
    private TrackResult latestResult;
    private bool newDataReceived = false;

    private bool shuttingDown = false;

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
                    TrackPacket packet =
                        JsonUtility.FromJson<TrackPacket>(json);

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

        TrackResult data;

        lock (dataLock)
        {
            data = latestResult;
            newDataReceived = false;
        }

        // Unity 메인 스레드에서 안전하게 사용
        //Debug.Log(
        //    $"Pose:{data.has_pose} " +
        //    $"Left:{data.left_value:F2}({data.left_label}) " +
        //    $"Right:{data.right_value:F2}({data.right_label}) " +
        //    $"Drive:{data.drive_label}");

        // 예시: 탱크 제어
        if (data.has_pose)
        {
            leftTrack = data.left_value;
            rightTrack = data.right_value;

            // TankController.SetTrackInput(leftTrack, rightTrack);
        }
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