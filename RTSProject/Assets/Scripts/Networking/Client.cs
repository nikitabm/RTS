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
using Newtonsoft.Json;

public class Client : MonoBehaviour
{

    public string IP = "localhost";
    public int id;
    public TcpClient socketConnection;
    private Server _server;
    private Thread clientReceiveThread;
    private Thread clientSendThread;
    private NetworkStream stream;
    private bool isTrue;
    public bool connected;
    public bool host;
    private int port = 55555;
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


    private void Start()
    {
        myDataConfirmed = false;
        readyToTurnWheel = false;
        otherPlayerDataReceived = false;
        _turnState = TurnState.none;
        _clientState = ClientState.none;
        nm = ServiceLocator.GetService<NetworkingManager>();
        ConnectToTcpServer();
        OnMessageReceived += nm.DecodeMessage;
    }

    private void Update()
    {
        nm.ClientText.text = log;
        if (Input.GetKeyDown(KeyCode.Space))
        {

            var turnData = ServiceLocator.GetService<CommandManager>().CreateTurnData(2, 0);
            string msg = JsonConvert.SerializeObject(turnData);
            SendMessage(msg);
        }
    }

    private void OnApplicationQuit()
    {
        DisconnectFromServer();
    }

    public void ConnectToTcpServer()
    {
        isTrue = true;
        socketConnection = new TcpClient("127.0.0.1", port);
        stream = socketConnection.GetStream();
        _clientState = ClientState.Connected;
        string line = "Connected to: " + IP + ":" + port.ToString();
        Debug.Log(line);
        log += line + Environment.NewLine;
        clientReceiveThread = new Thread(new ThreadStart(ListenForData));
        clientReceiveThread.IsBackground = true;
        clientReceiveThread.Start();
    }

    public void DisconnectFromServer()
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

    private void SendData()
    {

    }
    private void ListenForData()
    {
        Byte[] bytes = new Byte[1024];
        while (isTrue)
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
            else
            {
                log += "stream cannot write" + Environment.NewLine;
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);

        }
    }
}