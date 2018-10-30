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
public class TCPServer : MonoBehaviour
{


    public int port = 8888;

    private PlayerController _pcRef;
    private NetworkingManager _nm;
    private List<ServerClient> clients;
    private List<ServerClient> disconnects;
    private TcpListener server;
    private StreamWriter writer;
    private StreamReader reader;
    private bool started = false;

    public void Start()
    {
        clients = new List<ServerClient>();
        disconnects = new List<ServerClient>();

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            StartListening();
            started = true;
            Debug.Log("Server started on port: " + port.ToString());
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e.Message);
        }

    }
    private void SendMessage(NetworkStream pStream, byte[] pMessage)
    {
        pStream.Write(BitConverter.GetBytes(pMessage.Length), 0, 4);
        pStream.Write(pMessage, 0, pMessage.Length);
    }
    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;
        clients.Add(new ServerClient(listener.EndAcceptTcpClient(ar)));
        Debug.Log("New connection established");
        StartListening();
    }

    private bool isConnected(TcpClient c)
    {
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
                return true;
            else
                return false;
        }
        catch
        {
            return false; //not able to reach client
        }

    }

    private void onIncomingData(ServerClient c, string data)
    {
        Debug.Log(c.clientName + "Has sent the following message: " + data);
        byte[] b = new byte[3];
        b[0] = 0;
        b[1] = 1;
        b[2] = 1;

        writer = new StreamWriter(c.tcp.GetStream());
        writer.WriteLine("test");

    }


    private void Update()
    {
        if (!started)
            return;

        foreach (ServerClient c in clients)
        {
            //is the client still connected
            if (!isConnected(c.tcp))
            {
                c.tcp.Close();
                disconnects.Add(c);
                continue;
            }

            //check for message
            NetworkStream s = c.tcp.GetStream();
            if (s.DataAvailable)
            {
                print(TCPHelper.ReceiveString(s, Encoding.UTF8));
                TCPHelper.SendString(s, "CONFIRMED", Encoding.UTF8);

            }
        }
    }

    public int GetConnectedClients()
    {
        if (clients == null) return 0;
        return clients.Count;
    }
    public void Stop()
    {
        server.Stop();
    }
}


