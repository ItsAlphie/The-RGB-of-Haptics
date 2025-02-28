using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

public class CommunicationController : MonoBehaviour
{
    private const int listenPort = 11000;
    UdpClient listener = new UdpClient(listenPort);
    IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
    IPAddress espIP = IPAddress.Parse("192.168.0.0");

    public ConcurrentQueue<float> fingerData = new ConcurrentQueue<float>();

    private static CommunicationController _instance;

    public static CommunicationController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("CommunicationController instance is null EEEEEeeeeeEEEEEeeeeeeeeEeeEeeeeeeeeEEEEEeeee");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        print("Starting Communication thread");
        Thread msgThread = new Thread(ReceiveMessages);
        msgThread.Start();
    }

    private void ReceiveMessages()
    {
        try
        {
            while (true)
            {
                byte[] bytes = listener.Receive(ref groupEP);
                string senderIP = groupEP.Address.ToString();
                print("Got Data");
                ProcessMsg(bytes);
            }
        }
        catch (SocketException e)
        {
            print(e);
        }
    }

    void Update()
    {
        if (FingerTracking.Instance != null) {
            while (fingerData.TryDequeue(out float data)){
                print("Data dequeued");
                FingerTracking.Instance.updatePose(data);
            }
        }
    }
    public void SendMsg(string msg)
    {
        try
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            byte[] sendbuf = Encoding.ASCII.GetBytes(msg);
            IPEndPoint ep = new IPEndPoint(espIP, listenPort);

            s.SendTo(sendbuf, ep); s.Close();
        }
        catch (SocketException e)
        {
            print(e);
        }
    }

    public void ProcessMsg(byte[] bytes)
    {
        string message = System.Text.Encoding.UTF8.GetString(bytes);
        print(message);
        string[] values = message.Split(',');

        float[] data = new float[3];
        for (int i = 0; i < Marshal.SizeOf(values); i++)
        {
            data[i] = float.Parse(values[i]);
        }

        switch (data[0])
        {
            case 1f:
                // Peltier
                break;
            case 2f:
                // Vibration
                break;
            case 3f:
                // Force
                break;
            case 4f:
                // Flex Sensor Data
                print("Message received: Flex Sensor Data");
                fingerData.Enqueue(data[1]);
                break;
            default:
                print("Message received, but incorrect index");
                break;
        }
    }
}