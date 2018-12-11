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
public class Client : MonoBehaviour
{

    public string IP = "localhost";
    public int id;
    public TcpClient socketConnection;
    private Server _server;
    private Thread clientReceiveThread;
    private Thread clientSendThread;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    private bool isTrue;
    public bool connected;
    public bool host;
    private int port = 55555;
    private bool once;
    public bool otherPlayerDataReceived;
    public bool readyToTurnWheel;
    public bool myDataConfirmed;
    NetworkingManager nm;
    public delegate void MessageReceived(string s);
    public static event MessageReceived OnMessageReceived;
    string log;
    public enum TurnState
    {
        none,
        DataSent,
        WaitingForOtherPlayerDataAndConformation,
        WaitingForOtherPlayerData,
        WaitingForOtherPlayerConformation,
        DataComplete
    }
    public TurnState _turnState;
    public enum ClientState
    {
        none,
        Waiting,
        Connected,
        InGame,
        Preparing,
        Playing
    }
    public Server Server
    {
        get
        {
            return _server;
        }
        set
        {
            _server = value;
        }
    }
    public ClientState _clientState;


    void Start()
    {
        myDataConfirmed = false;
        readyToTurnWheel = false;
        otherPlayerDataReceived = false;
        once = false;
        _turnState = TurnState.none;
        _clientState = ClientState.none;
        nm = ServiceLocator.GetService<NetworkingManager>();
        ConnectToTcpServer();
        OnMessageReceived += nm.DecodeMessage;
    }

    void Update()
    {
        nm.ClientText.text = log;
        if (Input.GetKeyDown(KeyCode.S))
        {
            SendMessage("Hello from client!");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            print(clientReceiveThread.IsAlive);
        }
    }
    void OnApplicationQuit()
    {
        try
        {
            socketConnection.Close();

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        try
        {
            isTrue = false;
            clientReceiveThread.IsBackground = false;
            clientReceiveThread.Abort();
            clientSendThread.IsBackground = false;
            clientSendThread.Abort();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void ConnectToTcpServer()
    {

        try
        {
            socketConnection = new TcpClient("127.0.0.1", port);
            stream = socketConnection.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            _clientState = ClientState.Connected;
            string line = "Connected to: " + IP + ":" + port.ToString();
            Debug.Log(line);
            log += line + Environment.NewLine;
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void SendData()
    {

    }
    private void ListenForData()
    {


        Byte[] bytes = new Byte[1024];
        while (true)
        {
            try
            {
                stream = socketConnection.GetStream();
                int length;
                while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    var incommingData = new byte[length];
                    Array.Copy(bytes, 0, incommingData, 0, length);
                    string serverMessage = Encoding.ASCII.GetString(incommingData);
                    log += "server message received as: " + serverMessage + Environment.NewLine;
                    OnMessageReceived(serverMessage);
                }
            }
            catch (SocketException socketException)
            {
                Debug.Log("Socket exception: " + socketException);
            }
        }

    }

    public new void SendMessage(string s)
    {
        if (socketConnection == null)
        {
            print("Socket is null");
            return;
        }
        try
        {
            stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                print("sending msg from client to server");

                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(s);
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);

        }
    }
}