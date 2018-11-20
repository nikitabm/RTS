﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading;

public class TCPTestServer : MonoBehaviour
{
    #region private members 	
    /// <summary> 	
    /// TCPListener to listen for incomming TCP connection 	
    /// requests. 	
    /// </summary> 	
    private TcpListener tcpListener;
    /// <summary> 
    /// Background thread for TcpServer workload. 	
    /// </summary> 	
    private Thread TcpListenerThread;
    private Thread TcpClientAcceptThread;
    private Thread TcpSendThread;

    /// <summary> 	
    /// Create handle to connected tcp client. 	
    /// </summary> 	
    private TcpClient connectedTcpClient;
    private List<ServerClient> clients;
    private List<ServerClient> disconnects;
    public TcpTestClient localClient;

    private int port = 55555;
    private bool started;
    private bool AllPlayersConnected;
    string log;
    private enum GameState
    {
        none,
        WaitingForPlayers,
        GameStart,
        GamePause
    }
    private GameState _gameState;
    public delegate void ServerSendAction();
    public static ServerSendAction OnSend;
    NetworkingManager nm;

    #endregion

    void Start()
    {
        _gameState = GameState.none;
        clients = new List<ServerClient>();
        started = false;
        try
        {
            tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 55555);
            tcpListener.Start();
            TcpClientAcceptThread = new Thread(new ThreadStart(StartListening));
            TcpClientAcceptThread.IsBackground = true;
            TcpClientAcceptThread.Start();

            TcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
            TcpListenerThread.IsBackground = true;
            TcpListenerThread.Start();
            log += "Server initialized" + Environment.NewLine;
            nm = (ServiceLocator.GetService(typeof(NetworkingManager)) as NetworkingManager);


        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    public void Update()
    {
        nm.serverText.text = log;
        if (Input.GetKeyDown(KeyCode.K))
        {
            print(TcpListenerThread.IsAlive);
            print(TcpClientAcceptThread.IsAlive);


        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            foreach (ServerClient c in clients)
            {
                SendMessage(c.tcp, "Hello from server to " + clients.IndexOf(c).ToString());
            }
        }
    }
    private void SendData()
    {
    }
    private void StartListening()
    {
        while (true)
        {
            if (tcpListener.Pending())
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                clients.Add(new ServerClient(client));
                print("Server registered client to client list");
                log += "registered client to client list, client count: " + clients.Count + Environment.NewLine;

                print(clients.Count);
            }

        }
    }

    void OnApplicationQuit()
    {
        try
        {
            tcpListener.Stop();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        try
        {
            TcpClientAcceptThread.IsBackground = false;
            TcpClientAcceptThread.Abort();
            TcpListenerThread.IsBackground = false;
            TcpListenerThread.Abort();
            TcpSendThread.IsBackground = false;
            TcpSendThread.Abort();

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /// <summary> 	
    /// Runs in background TcpServerThread; Handles incomming TcpClient requests 	
    /// </summary> 	
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
    private bool inClientList(TcpClient connectedTcpClient)
    {
        for (int i = 0; i < clients.Count - 1; i++)
        {
            if (connectedTcpClient == clients[i].tcp)
            {

            }
            else return false;
        }
        return true;
    }
    private void ListenForIncommingRequests()
    {
        // try
        // {
        Byte[] bytes = new Byte[1024];

        while (true)
        {


            for (int i = 0; i < clients.Count; i++)
            {
                int length = 0;
                if (clients[i].tcp.Available > 0)
                {

                    //?
                    length = clients[i].tcp.GetStream().Read(bytes, 0, bytes.Length);

                    var incommingData = new byte[length];
                    Array.Copy(bytes, 0, incommingData, 0, length);
                    string clientMessage = Encoding.ASCII.GetString(incommingData);
                    string s = "server receives msg from client # " + i + ": " + clientMessage;
                    log += s + Environment.NewLine;
                }
            }
        }
    }






    public void SendMessageToClient(string s)
    {
        SendMessage(clients[1].tcp, s);
    }
    private void SendTestMsg()
    {
        if (clients[0] != null)
            SendMessage(clients[0].tcp, "Hello from Server To client 0");
        if (clients[1] != null)
            SendMessage(clients[1].tcp, "Hello from Server To client 1");

    }
    private void SendMessage(TcpClient c, string s)
    {
        if (c == null)
        {
            print("client is null, returning...");
            return;
        }

        try
        {
            NetworkStream stream = c.GetStream();
            if (stream.CanWrite)
            {
                string serverMessage = s;
                byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
                stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}
public class ServerClient
{
    public TcpClient tcp;
    public string clientName;

    public ServerClient(TcpClient clientSocket)
    {
        clientName = "Guest";
        tcp = clientSocket;
    }
}