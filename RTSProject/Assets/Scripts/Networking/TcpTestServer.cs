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

    private int port = 55555;
    private bool started;
    private NetworkStream stream;
    private StreamReader reader;
    private StreamWriter writer;


    #endregion

    // Use this for initialization
    void Start()
    {
        clients = new List<ServerClient>();
        started = false;
        try
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            StartListening();
            started = true;

            TcpClientAcceptThread = new Thread(new ThreadStart(StartListening));
            TcpClientAcceptThread.IsBackground = true;
            TcpClientAcceptThread.Start();

            tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
            tcpListenerThread.IsBackground = true;
            tcpListenerThread.Start();
            print("Server initialized");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    private void StartListening()
    {
        if (clients != null && clients.Count <= 2)
        {
            print("Server registered client to client list");
            clients.Add(new ServerClient(tcpListener.AcceptTcpClient()));
        }
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            // SendMessage(clients[0].tcp);
            print(tcpListenerThread.IsAlive);

        }
    }
    // private void AcceptTcpClient(IAsyncResult ar)
    // {
    //     TcpListener listener = (TcpListener)ar.AsyncState;
    //     clients.Add(new ServerClient(listener.EndAcceptTcpClient(ar)));
    //     print("Number of clients: " + clients.Count);
    //     print("New connection established");
    //     //StartListening();
    // }

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
            tcpListenerThread.IsBackground = false;
            tcpListenerThread.Abort();
            TcpClientAcceptThread.IsBackground = false;
            TcpClientAcceptThread.Abort();
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
        try
        {
            Byte[] bytes = new Byte[1024];
            while (started)
            {
                foreach (ServerClient c in clients)
                {
                    if (!isConnected(clients[0].tcp))
                    {
                        print("closing client");
                        clients[0].tcp.Close();
                        disconnects.Add(clients[0]);
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
                        PlayerCommandsData command = JsonUtility.FromJson<PlayerCommandsData>(clientMessage);
                        int turn = command.turn;
                        int playerID = command.playerID;
                        CustomMoveCommand cm = command.moveCommand;
                        SendMessage(c.tcp);
                        // (ServiceLocator.GetService(typeof(LockStepManager)) as LockStepManager).
                        // AllPlayersTurns.Add(turn, new AllPlayersCommandsData(playerID, cm));

                        //print
                        print("almost there");
                        // print((ServiceLocator.GetService(typeof(LockStepManager)) as LockStepManager).
                        // AllPlayersTurns[turn]);

                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("SocketException " + socketException.ToString());
        }
    }
    /// <summary> 	
    /// Send message to client using socket connection. 	
    /// </summary> 	
    private void SendMessage(TcpClient c)
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
                string serverMessage = "server msg: I confirm";
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