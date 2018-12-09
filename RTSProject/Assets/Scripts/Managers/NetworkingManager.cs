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
    private Player _playerRef;


    //TODO: 
    //1. game opens;
    //2. press host game+create client;
    //   a) player is created in GameManager;
    //-> player runs select objects script that sends objects that are created to command manager;
    //->command manager starts sending commands to server when server tells player that game can start;
    //   b) server runs and creates lockstepManager;
    //   c) lockstepManager waits for both players to connect to start lockstepping;
    //3. when second player connects, server send command to both players that game is ready;
    //4. command manager starts listening for commands, network manager starts sending commands from queue;



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
    private void ClientSend()
    {
        //TODO:
        // _cl.send
        //client send data to server method
        //make it generic
    }
    private void ServerSend()
    {
        //TODO:
        //same but for server
    }
    private void CreatePlayer()
    {
        _playerRef = ServiceLocator.GetService<GameManager>().CreatePlayer();
    }
    public void DecodeMessage(string s)
    {
        if (_cl != null)
            if (s.Length == 1)
            {
                _cl.id = (int)char.GetNumericValue(s.ToCharArray()[0]);
                print(_cl.id);
            }
    }
    // public override int DecodeMessage(string s)
    // {
    //     if (_cl != null)
    //         if (s.Length == 1)
    //         {
    //             return (int)char.GetNumericValue(s.ToCharArray()[0]);
    //             // print(_cl.id);
    //         }
    //         else return -1;
    //     else
    //         return -1;
    // }
    private void Update()
    {

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
            _sr = gameObject.AddComponent<Server>();
            _cl = gameObject.AddComponent<Client>();
            CreatePlayer();
        }

    }
    public void ConnectToGame()
    {
        if (_cl == null)
        {
            host = false;
            _cl = gameObject.AddComponent<Client>();
            CreatePlayer();
        }
    }
    #endregion
}
