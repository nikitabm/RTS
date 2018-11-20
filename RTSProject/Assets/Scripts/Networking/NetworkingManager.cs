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
    public Text serverText;
    public Text ClientText;

    private Queue<Command> commandQueue = new Queue<Command>();

    //private
    private TCPServer _server = null;
    private TcpTestClient _cl;
    private TCPTestServer _sr;
    private bool host = false;

    void Start()
    {
        ServiceLocator.ProvideService(this);
    }
    public TCPServer GetServer()
    {
        if (host)
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
    public TcpTestClient GetOwningTCPClient()
    {
        return _cl;
    }
    private void SendCommands()
    {
        // based on host value send command to host or to client...(or make it generic??)
    }

    #region MethodsForButtons
    public void RunServer()
    {
        if (_sr == null)
        {
            host = true;
            _sr = gameObject.AddComponent<TCPTestServer>();
        }
    }
    public void HostGame()
    {
        if (_sr == null && _cl == null)
        {
            host = true;
            _cl = gameObject.AddComponent<TcpTestClient>();
            _sr = gameObject.AddComponent<TCPTestServer>();
        }

    }
    public void ConnectToGame()
    {
        if (_cl == null)
        {
            host = false;
            _cl = gameObject.AddComponent<TcpTestClient>();
        }
    }
    #endregion
}
