﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

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
    private int port = 55555;
    private NetworkStream stream;

    #endregion

    // Use this for initialization
    void Start()
    {
        tcpListener = new TcpListener(IPAddress.Any, port);
        // Start TcpServer background thread 		
        tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendMessage();
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
    private void ListenForIncommingRequests()
    {
        try
        {
            // Create listener on localhost port 8052. 	
            tcpListener.Start();
            connectedTcpClient = tcpListener.AcceptTcpClient();
            stream=connectedTcpClient.GetStream();
            Debug.Log("Server is listening");
            Byte[] bytes = new Byte[1024];
            while (true)
            {

                // Get a stream object for reading 					
                int length;
                // Read incomming stream into byte arrary. 						
                while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    var incommingData = new byte[length];
                    Array.Copy(bytes, 0, incommingData, 0, length);
                    // Convert byte array to string message. 							
                    string clientMessage = Encoding.ASCII.GetString(incommingData);
                    Debug.Log("server receives: " + clientMessage);
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
    private void SendMessage()
    {
        if (connectedTcpClient == null)
        {
            return;
        }

        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = connectedTcpClient.GetStream();
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