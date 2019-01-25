using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading;

public class Server : MonoBehaviour
{

    private TcpListener tcpListener;

    private Thread TcpListenerThread;
    private Thread TcpClientAcceptThread;
    private Thread TcpSendThread;


    private TcpClient connectedTcpClient;
    private List<ServerClient> clients;
    private List<ServerClient> disconnects;
    private LockStepManager _lockStepManager;
    public Client localClient;

    private int port = 55555;
    private bool started;
    private bool AllPlayersConnected;
    public string log;

    private bool _playersConnected;
    public delegate void ServerAcceptClient(TcpClient t, string s);
    public static event ServerAcceptClient OnAccept;

    public delegate void BothPlayersConnected();
    public static event BothPlayersConnected OnAllPlayersConnected;
    public delegate void ReceivedMessage(string s);
    public static event ReceivedMessage OnMessageReceive;
    NetworkingManager nm;

    void Start()

    {
        OnMessageReceive += ServiceLocator.GetService<NetworkingManager>().DecodeServerMessage;
        clients = new List<ServerClient>();
        OnAccept += SendMessage;
        _lockStepManager = gameObject.AddComponent<LockStepManager>();
        //OnAllPlayersConnected += SendTestMsg;
        OnAllPlayersConnected += _lockStepManager.StartGame;
        LockStepManager.NextTurn += incTurns;
        RunServer();
    }

    public void incTurns()
    {
        SendMessage(clients[0].tcp, "inc");
        SendMessage(clients[1].tcp, "inc");
    }

    public void SendMessageToClients(string s, string s1)
    {
        SendMessage(clients[0].tcp, s);
        SendMessage(clients[1].tcp, s1);
    }

    public void RunServer()
    {

        started = true;

        tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
        tcpListener.Start();
        TcpClientAcceptThread = new Thread(new ThreadStart(StartListening));
        TcpClientAcceptThread.IsBackground = true;
        TcpClientAcceptThread.Start();

        TcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
        TcpListenerThread.IsBackground = true;
        TcpListenerThread.Start();
        log += "Server initialized" + Environment.NewLine;
        nm = ServiceLocator.GetService<NetworkingManager>();

        TcpSendThread = new Thread(new ThreadStart(SendData));
        TcpSendThread.IsBackground = true;
        TcpSendThread.Start();

    }

    public void StopServer()
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
            _playersConnected = false;
            started = false;
            TcpClientAcceptThread.IsBackground = false;
            TcpClientAcceptThread.Abort();
            TcpListenerThread.IsBackground = false;
            TcpListenerThread.Abort();
            TcpSendThread.IsBackground = false;
            TcpSendThread.Abort();
            clients.Clear();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void Update()
    {
        nm.serverText.text = log;
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    var data = new PlayerCommandsData(3000, 1000);
        //    data.AddCommand(new BuildCommand(5, new List<int> { 1, 2, 3, 4 }, new Vector3(4, 4, 4)));
        //    SendMessage(clients[0].tcp, Newtonsoft.Json.JsonConvert.SerializeObject(data));
        //}
    }
    private void SendData()
    {
    }
    private void StartListening()
    {
        while (!_playersConnected)
        {
            if (tcpListener.Pending())
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                ServerClient cl = new ServerClient(client);
                clients.Add((cl));
                print("Server registered client to client list");
                log += "registered client to client list, client count: " + clients.Count + Environment.NewLine;
                print(clients.Count);
                OnAccept(client, clients.IndexOf(cl).ToString());
                if (clients.Count == 2)
                {
                    _playersConnected = true;
                    OnAllPlayersConnected();
                }
            }

        }
    }

    private void OnApplicationQuit()
    {
        StopServer();
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
            return false;
        }

    }
    private void ListenForIncommingRequests()
    {

        Byte[] bytes = new Byte[1024];

        while (started)
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
                    OnMessageReceive(clientMessage);
                }
            }
        }
    }


    private void SendTestMsg()
    {
        if (clients[0] != null)
            SendMessage(clients[0].tcp, "go");
        if (clients[1] != null)
            SendMessage(clients[1].tcp, "go");

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
    private void SendMessage(TcpClient c, NetworkHelper.ServerCommand command)
    {

    }
}
public class ServerClient
{
    public TcpClient tcp;
    public string clientName;
    public int id;

    public ServerClient(TcpClient clientSocket)
    {
        clientName = "Guest";
        tcp = clientSocket;
    }
}