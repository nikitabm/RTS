using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Security;
using System.Linq;
using System.Text;
using System.Threading;

public class TCPClient : MonoBehaviour
{

    private bool connected = false;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    public string IP = "localhost";
    public int port = 8888;
    private int count = 0;

    public void connectToServer()
    {
        if (connected)    //already connected
            return;

        try
        {

            socket = new TcpClient(IP, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            connected = true;
            Debug.Log("Connected to: " + IP + ":" + port.ToString());
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e.Message);
        }
    }

    private void Send()
    {
        // String Data = Time.time.ToString();
        // if (!connected)
        //     return;
        // writer.WriteLine(Data);
        // writer.Flush();
        // count = count + 1;
        TCPHelper.SendString(stream,"Hello I am client",Encoding.UTF8);
    }

    void Start()
    {
        Invoke("connectToServer", 1.0f);
        // InvokeRepeating("Send", 0.0f, 0.5f);
    }

    void Update()
    {
        if (!connected) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Send();

        }
        print(TCPHelper.ReceiveString(stream,Encoding.UTF8));
    }
}
