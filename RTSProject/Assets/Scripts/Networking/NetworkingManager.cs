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
    private TcpTestClient _cl;
    private TCPTestServer _sr;
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

    private bool host = false;
    private bool connection = false;
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
    public PlayerController GetOwningPC()
    {
        return _pc;
    }
    public void HostGame()
    {
        host = true;
        _sr = gameObject.AddComponent<TCPTestServer>();
        _cl = gameObject.AddComponent<TcpTestClient>();
        _cl.Server = _sr;
        _cl.host = true;
        (ServiceLocator.GetService(typeof(LockStepManager)) as LockStepManager).client = _cl;
    }
    public void ConnectToGame()
    {
        host = false;
        _cl = gameObject.AddComponent<TcpTestClient>();
        _cl.host = false;
        (ServiceLocator.GetService(typeof(LockStepManager)) as LockStepManager).client = _cl;
    }
}
