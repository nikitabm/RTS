using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NetworkingManager : MonoBehaviour, Service
{


    //public
    public GameObject playerController;


    //private

    private PlayerController _pc;
    //=============================
    private TCPServer _server = null;
    private TCPClient _localCLient = null;
    //=============================

    private TcpListener _tcpListener;
    /// <summary> 
    /// Background thread for TcpServer workload. 	
    /// </summary> 	
    private Thread _tcpListenerThread;
    /// <summary> 	
    /// Create handle to connected tcp client. 	
    /// </summary> 	
    private TcpClient _connectedTcpClient;

    private bool host=false;
    //local client
    private TcpClient _socketConnection;
    private Thread _clientReceiveThread;


    void Start()
    {
        ServiceLocator.ProvideService(this);
        _pc = playerController.GetComponent<PlayerController>();
    }
    public TCPServer GetServer()
    {
        if(host)
        return _server;
        else return null;
    }
    public bool HasAuthority()
    {
        return host;
    }
    public void GoToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public TCPClient GetOwningTCPClient()
    {
        return _localCLient;
    }
    public PlayerController GetOwningPC()
    {
        return _pc;
    }
    public void HostGame()
    {
        host=true;
        _server = gameObject.AddComponent(typeof(TCPServer)) as TCPServer;
        _localCLient = gameObject.AddComponent(typeof(TCPClient)) as TCPClient;
    }
    public void ConnectToGame()
    {
        host=false;
        _localCLient = gameObject.AddComponent(typeof(TCPClient)) as TCPClient;
    }
    public void Disconnect()
    {
        if (_server != null)
            _server.Stop();
        // if(localCLient!=null)
    }

    //=====================================

    public void StartServer()
    {
        // Start TcpServer background thread 		
        _tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
        _tcpListenerThread.IsBackground = true;
        _tcpListenerThread.Start();
    }

    //called once per unity frame

    private void ListenForData()
    {
        try
        {
            _socketConnection = new TcpClient("localhost", 8052);
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading 				
                using (NetworkStream stream = _socketConnection.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary. 					
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message. 						
                        string serverMessage = Encoding.ASCII.GetString(incommingData);
                        Debug.Log("server message received as: " + serverMessage);
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
    private void LogCommandtoDictionary(string s)
    {
        
    }
    private void ListenForIncommingRequests()
    {
        try
        {

            // Create listener on localhost port 8052. 			
            _tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8052);
            _tcpListener.Start();
            Debug.Log("Server is listening");
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                using (_connectedTcpClient = _tcpListener.AcceptTcpClient())
                {
                    // Get a stream object for reading 					
                    using (NetworkStream stream = _connectedTcpClient.GetStream())
                    {
                        int length;
                        // Read incomming stream into byte array. 						
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incommingData = new byte[length];
                            Array.Copy(bytes, 0, incommingData, 0, length);
                            //Convert  incomming data to command, save it into _pc.AllplayersTurns to the turn
                            //send conformation msg to clients and update
                            string clientMessage = Encoding.ASCII.GetString(incommingData);
                            Debug.Log("client message received as: " + clientMessage);
                            // _pc.AllPlayersTurns.
                        }
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
    private void ServerSendMessage()
    {
        if (_connectedTcpClient == null)
        {
            return;
        }

        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = _connectedTcpClient.GetStream();
            if (stream.CanWrite)
            {
                string serverMessage = "This is a message from your server.";
                // Convert string message to byte array.                 
                byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
                // Write byte array to socketConnection stream.               
                stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
                Debug.Log("Server sent his message - should be received by client");

            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
    private void ClientSendMessage()
    {
        if (_socketConnection == null)
        {
            System.Console.WriteLine("socket connection is null, returning...");
            return;
        }
        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = _socketConnection.GetStream();
            if (stream.CanWrite)
            {
                string clientMessage = "This is a message from one of your clients.";
                // Convert string message to byte array.                 
                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                // Write byte array to socketConnection stream.                 
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                Debug.Log("Client sent his message - should be received by server");

            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}
