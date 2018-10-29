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
public class TcpTestClient : MonoBehaviour
{

    public string IP = "localhost";

    #region private members 	
    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    private bool isTrue;
    private bool connected;
    private int port = 55555;


    #endregion
    // Use this for initialization 	
    void Start()
    {
        Invoke("ConnectToTcpServer", 1.0f);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            SendMessage("Test");
        }

    }
    void OnApplicationQuit()
    {
        try
        {
            socketConnection.Close();
            isTrue = false;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        // You must close the tcp listener
        try
        {
            clientReceiveThread.IsBackground = false;
            clientReceiveThread.Abort();
            isTrue = false;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    /// <summary> 	
    /// Setup socket connection. 	
    /// </summary> 	
    public void ConnectToTcpServer()
    {

        try
        {
            socketConnection = new TcpClient(IP, port);
            stream=socketConnection.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            connected = true;
            Debug.Log("Connected to: " + IP + ":" + port.ToString());
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    /// <summary> 	
    /// Runs in background clientReceiveThread; Listens for incomming data. 	
    /// </summary>  

    private void ListenForData()
    {
        try
        {
            
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                print("CLIENT: getting to listening for data");
                // Get a stream object for reading 				
                stream = socketConnection.GetStream();
                int length;
                // Read incomming stream into byte arrary. 					
                while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    var incommingData = new byte[length];
                    Array.Copy(bytes, 0, incommingData, 0, length);
                    // Convert byte array to string message. 						
                    string serverMessage = Encoding.ASCII.GetString(incommingData);
                    Debug.Log("server message received at: " + serverMessage);
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
    /// <summary> 	
    /// Send message to server using socket connection. 	
    /// </summary> 	
    public new void SendMessage(string s)
    {
        if (socketConnection == null)
        {
            System.Console.WriteLine("socket connection is null, returning...");
            return;
        }
        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                print("sending msg from client to server");
                // Convert string message to byte array.                 
                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(s);
                // Write byte array to socketConnection stream.                 
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                stream.Flush();
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}