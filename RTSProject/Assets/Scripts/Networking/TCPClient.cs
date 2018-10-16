using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Security;
public class TCPClient : MonoBehaviour
{

    //public
    public string IP = "localhost";
    public int port = 8888;

    //private 
    private PlayerController _pcRef;
    private NetworkingManager _nm;
    private int count = 0;
    private bool connected = false;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    private void Start()
    {
        Invoke("connectToServer", 1.0f);
        // InvokeRepeating("Send", 0.0f, 0.5f);
    }
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
        String Data = Time.time.ToString();
        if (!connected)
            return;
        writer.WriteLine(Data);
        writer.Flush();
        // count = count + 1;
    }

    

    private void SendCommand()
    {
        
        _nm=ServiceLocator.GetService(typeof(NetworkingManager)) as NetworkingManager;
        _pcRef=_nm.GetComponent<NetworkingManager>().GetOwningPC();
        writer.WriteLine(_pcRef._commandList[count]._position);
        writer.Flush();
        count++;
    }
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            SendCommand();
        }
        // if (count >= 20)
        //     CancelInvoke();
    }
}
