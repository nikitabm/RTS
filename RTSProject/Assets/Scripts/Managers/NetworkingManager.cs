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



    //private
    private Client _cl;
    private Server _sr;
    private bool host = false;

    //command sending related variables
    private float FrameLength = 1.0f; //should be 50 ms
    private float AccumilatedTime = 0f;



    void Start()
    {
        ServiceLocator.ProvideService(this);
    }

    public bool HasAuthority()
    {
        return host;
    }
    public void GoToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public Client GetOwningTCPClient()
    {
        return _cl;
    }
    private void SendCommands()
    {
        //make it generic
    }
    private void CreatePlayer()
    {
        
    }
    private void Update()
    {
        //Basically same logic as FixedUpdate, but we can scale it by adjusting FrameLength
        AccumilatedTime = AccumilatedTime + Time.deltaTime;

        //in case the FPS is too slow, we may need to update the game multiple times a frame
        while (AccumilatedTime > FrameLength)
        {
            //when turn is finished-send all "Player commands Data" to server
            //inc "current turn for writing data" or sth like that
            AccumilatedTime = AccumilatedTime - FrameLength;
        }
    }

    #region Methods Called From Buttons
    public void RunServer()
    {
        if (_sr == null)
        {
            host = true;
            _sr = gameObject.AddComponent<Server>();
        }
    }
    public void HostGame()
    {
        if (_sr == null && _cl == null)
        {
            host = true;
            _cl = gameObject.AddComponent<Client>();
            _sr = gameObject.AddComponent<Server>();
        }

    }
    public void ConnectToGame()
    {
        if (_cl == null)
        {
            host = false;
            _cl = gameObject.AddComponent<Client>();
        }
    }
    #endregion
}
