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
    /// <summary> 	
    /// Create handle to connected tcp client. 	
    /// </summary> 	
    private TcpClient connectedTcpClient;
    private List<ServerClient> clients;
    private int port = 8888;
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
            started = true;
            tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
            tcpListenerThread.IsBackground = true;
            tcpListenerThread.Start();
            print("Server initialized");
            StartListening();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    private void StartListening()
    {

        tcpListener.BeginAcceptTcpClient(AcceptTcpClient, tcpListener);
    }

    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;
        clients.Add(new ServerClient(listener.EndAcceptTcpClient(ar)));
        print("Number of clients: " + clients.Count);
        print("New connection established");
        StartListening();
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
        try
        {
            // Create listener on localhost port 8052. 	
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                foreach (ServerClient c in clients)
                {
                    if (!isConnected(c.tcp))
                    {
                        c.tcp.Close();
                        continue;
                    }

                    // Get a stream object for reading 					
                    int length;
                    // Read incomming stream into byte arrary. 		
                    while ((length = c.tcp.GetStream().Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message. 							
                        string clientMessage = Encoding.ASCII.GetString(incommingData);
                        Debug.Log("server receives: " + clientMessage);
                        SendMessage(c.tcp);

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
                Debug.Log("Server SENT msg...");
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