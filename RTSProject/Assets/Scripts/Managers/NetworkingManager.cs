﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class NetworkingManager : MonoBehaviour, Service
{


    public Text serverText;
    public Text ClientText;
    public Text turnText;
    public int turn;
    public float turnTime;

    //FIXME: change the way commands are processed
    public PlayerCommandsData playerOne;
    PlayerCommandsData playerTwo;
    public PlayerCommandsData newData;
    PlayerCommandsData turnData;
    public delegate void OnProccessCommands();
    public static OnProccessCommands proccessCommands;


    private Client _cl;
    private Server _sr;
    private bool host = false;
    private Player _playerRef;

    void Start()
    {
        turn = -2;
        turnText.text = "Turn: " + turn.ToString();
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

    private void CreatePlayer()
    {
        _playerRef = ServiceLocator.GetService<GameManager>().CreatePlayer();
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

    public void DecodeServerMessage(string s)
    {
        PlayerCommandsData playerData = JsonConvert.DeserializeObject<PlayerCommandsData>(s);
        if (playerData.PlayerID == 0)
        {
            playerOne = playerData;
            print("playerOneData");
        }
        if (playerData.PlayerID == 1)
        {
            playerTwo = playerData;
            print("playerTwoData");
        }
        if (playerOne != null && playerTwo != null)
        {
            print("send data to players");
            _sr.SendMessageToClients(JsonConvert.SerializeObject(playerTwo), JsonConvert.SerializeObject(playerOne));
            playerOne = null;
            playerTwo = null;

        }
    }
    public void DecodeMessage(string s)
    {
        if (_cl != null)
        {
            if (s.Length > 10)
            {
                PlayerCommandsData playerData = JsonConvert.DeserializeObject<PlayerCommandsData>(s);
                if (playerData.commands.Count > 0)
                    for (int i = 0; i < playerData.commands.Count; i++)
                    {
                        ServiceLocator.GetService<CommandManager>()._allCommands.Add(playerData.commands[i]);
                    }
                proccessCommands();
            }
            else
            {
                //TODO: change message checks
                if (s == "inc")
                {
                    turnData = ServiceLocator.GetService<CommandManager>().CreateTurnData(turn + 2, _cl.id);
                    if (turnData == null)
                    {
                        turnData = new PlayerCommandsData(turn + 2, _cl.id);
                        turnData.AddCommand(new MoveCommand(new List<int> { 0, 1 }, Vector3.zero));
                    }
                    string msg = JsonConvert.SerializeObject(turnData);
                    _cl.SendMessage(msg);
                    turn++;
                }
            }
        }
    }

    private void Update()
    {
        turnText.text = "Turn: " + turn.ToString();
    }

    #region Methods Called From Buttons

    public void HostServer()
    {
        if (_sr == null)
        {
            host = true;
            _sr = gameObject.AddComponent<Server>();
        }
    }

    public void HostServerAndClient()
    {
        if (_sr == null && _cl == null)
        {

            host = true;
            _sr = gameObject.AddComponent<Server>();
            _cl = gameObject.AddComponent<Client>();
            _cl.id = 0;
            CreatePlayer();
        }
    }

    public void CreateClientAndPlayer()
    {
        if (_cl == null)
        {
            host = false;
            _cl = gameObject.AddComponent<Client>();
            _cl.id = 1;
            CreatePlayer();
        }
        else
        {
            gameObject.AddComponent<Client>();
        }
    }
    public void ReconnectToServer()
    {
        _cl.ConnectToTcpServer();
    }
    public void ReHostServer()
    {
        _sr.RunServer();
    }
    public void DisconnectClient()
    {
        foreach (Client c in GetComponents<Client>())
        {
            c.DisconnectFromServer();
            Destroy(c);
        }
    }

    public void DisconnectServer()
    {
        _sr.StopServer();
        DisconnectClient();
        Destroy(_sr);
    }

    #endregion
}
