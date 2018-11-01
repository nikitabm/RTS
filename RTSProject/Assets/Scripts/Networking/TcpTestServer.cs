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
    private Thread tcpListenerThread;
    private Thread TcpClientAcceptThread;

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
    private enum GameState
    {
        none,
        WaitingForPlayers,
        GameStart,
        GamePause
    }
    private GameState _gameState;


    #endregion

    void Start()
    {
        _gameState = GameState.none;
        clients = new List<ServerClient>();
        started = false;
        try
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            started = true;

            TcpClientAcceptThread = new Thread(new ThreadStart(StartListening));
            TcpClientAcceptThread.IsBackground = true;
            TcpClientAcceptThread.Start();

            tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
            tcpListenerThread.IsBackground = true;
            tcpListenerThread.Start();
            print("Server initialized");
            _gameState = GameState.WaitingForPlayers;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            print(tcpListenerThread.IsAlive);
            print(TcpClientAcceptThread.IsAlive);

        }
    }

    private void StartListening()
    {
        while (!AllPlayersConnected)
        {
            Debug.Log("Waiting for a connection...");

            TcpClient client = tcpListener.AcceptTcpClient();
            if (client != null)
            {
                clients.Add(new ServerClient(client));
                print("Server registered client to client list");
                if (clients.Count >= 2)
                {
                    print("all players Connected");
                    _gameState = GameState.GameStart;
                    SendMessage(clients[1].tcp, "0");
                    AllPlayersConnected = true;
                    (ServiceLocator.GetService(typeof(NetworkingManager)) as NetworkingManager).
                    GetOwningTCPClient()._clientState = TcpTestClient.ClientState.InGame;
                }
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
            tcpListenerThread.IsBackground = false;
            tcpListenerThread.Abort();

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

    private void ListenForIncommingRequests()
    {
        while (true)
        {
            try
            {
                Byte[] bytes = new Byte[1024];

                foreach (ServerClient c in clients)
                {
                    if (!isConnected(c.tcp))
                    {
                        print("closing client");
                        c.tcp.Close();
                        disconnects.Add(c);
                        continue;
                    }
                    // Get a stream object for reading 					
                    int length = 0;
                    // Read incomming stream into byte arrary. 	
                    length = c.tcp.GetStream().Read(bytes, 0, bytes.Length);
                    print("Length: " + length);
                    if (length != 0)
                    {
                        print("getting here");
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message. 							
                        string clientMessage = Encoding.ASCII.GetString(incommingData);
                        print("server receives: " + clientMessage);
                        if (clientMessage == "2")
                        {
                            print("receiving command");
                            localClient._turnState = TcpTestClient.TurnState.DataComplete;
                        }
                        else if (clientMessage == "HELLO I AM CLIENT")
                        {
                            print("its ok now");
                        }
                        else
                        {
                            PlayerCommandsData command = JsonUtility.FromJson<PlayerCommandsData>(clientMessage);
                            if (command.command == -1) print("server receives empty command");
                            else if (command.command == 0) print("server recieves movecommand");

                            int receivedPlayerTurn = command.turn;
                            int receivedPlayerID = command.playerID;
                            print("server receives turn number: " + receivedPlayerTurn);
                        }


                        //still  need to add command or to send it to local client
                        // (ServiceLocator.GetService(typeof(LockStepManager)) as LockStepManager).
                        // AllPlayersTurns.Add(receivedPlayerTurn,
                        // new AllPlayersCommandsData(receivedPlayerID, new CustomMoveCommand(command.units, command.pos)));

                        // print((ServiceLocator.GetService(typeof(LockStepManager)) as LockStepManager).
                        // AllPlayersTurns[receivedPlayerTurn]);

                    }
                }
            }

            catch (SocketException socketException)
            {
                Debug.Log("SocketException " + socketException.ToString());
            }
        }
    }
    /// <summary> 	
    /// Send message to client using socket connection. 	
    /// </summary> 	
    public void SendMessageToClient(string s)
    {
        SendMessage(clients[1].tcp, s);
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
            // Get a stream object for writing. 			
            NetworkStream stream = c.GetStream();
            if (stream.CanWrite)
            {
                string serverMessage = s;
                // Convert string message to byte array.                 
                byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
                // Write byte array to socketConnection stream.               
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