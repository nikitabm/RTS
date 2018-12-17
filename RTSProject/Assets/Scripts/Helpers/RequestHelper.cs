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

public class RequestHelper
{
    public enum Requests
    {
        StartGame,
        NextTurn,

    }
    public RequestHelper()
    {

    }
    public static void SendMessage(NetworkStream stream, string s)
    {
        try
        {
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
    public static byte[] ReadBytes(NetworkStream stream)
    {
        byte[] bytes = new byte[1024];
        int length = stream.Read(bytes, 0, bytes.Length);

        var incommingData = new byte[length];
        Array.Copy(bytes, 0, incommingData, 0, length);
        //string clientMessage = Encoding.ASCII.GetString(incommingData);
        return incommingData;
    }
    public static string ReadString(NetworkStream stream)
    {
        string s = Encoding.ASCII.GetString(ReadBytes(stream));
        return s;
    }
}
