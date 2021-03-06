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
    public delegate void ProccessingCommands();
    public static ProccessingCommands ProccessedCommands;


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
        }
        if (playerData.PlayerID == 1)
        {
            playerTwo = playerData;
        }
        if (playerOne != null && playerTwo != null)
        {
            _sr.log += "send data to players" + Environment.NewLine;
            _sr.SendMessageToClients(JsonConvert.SerializeObject(playerTwo), JsonConvert.SerializeObject(playerOne));
            playerOne = null;
            playerTwo = null;

        }
    }
    public void DecodeMessage(string s)
    {
        if (_cl != null)
        {

            if (s.Length > 5)
            {
                PlayerCommandsData playerData = JsonConvert.DeserializeObject<PlayerCommandsData>(s);
                if (playerData.commands != null)
                {
                    for (int i = 0; i < playerData.commands.Count; i++)
                    {
                        int type = int.Parse(playerData.commands[i][0].ToString());
                        string lCommand = playerData.commands[i].Remove(0, 1);
                        print(playerData.commands[i]);
                        switch (type)
                        {
                            case 0:
                                AttackCommand c = JsonConvert.DeserializeObject<AttackCommand>(lCommand);
                                ServiceLocator.GetService<CommandManager>().allCommands.Add(c);
                                break;
                            case 1:
                                BuildCommand bc = JsonConvert.DeserializeObject<BuildCommand>(lCommand);
                                ServiceLocator.GetService<CommandManager>().allCommands.Add(bc);
                                print(ServiceLocator.GetService<CommandManager>().allCommands.Count);

                                break;
                            case 2:
                                HireCommand hc = JsonConvert.DeserializeObject<HireCommand>(lCommand);
                                ServiceLocator.GetService<CommandManager>().allCommands.Add(hc);

                                break;
                            case 3:
                                PauseCommand pc = JsonConvert.DeserializeObject<PauseCommand>(lCommand);
                                ServiceLocator.GetService<CommandManager>().allCommands.Add(pc);

                                break;
                            case 4:
                                MoveCommand mc = JsonConvert.DeserializeObject<MoveCommand>(lCommand);
                                ServiceLocator.GetService<CommandManager>().allCommands.Add(mc);

                                break;
                            case 5:
                                EmptyCommand ec = JsonConvert.DeserializeObject<EmptyCommand>(lCommand);
                                ServiceLocator.GetService<CommandManager>().allCommands.Add(ec);
                                break;
                        }
                        //Command c = (Command)JsonConvert.DeserializeObject(playerData.commands[i],
                        //    CommandManager.commandTypes[type].GetType());

                    }
                    ProccessedCommands();
                }
            }
            else
            {
                //TODO: change message checks
                if (s == "inc")
                {
                    turnData = ServiceLocator.GetService<CommandManager>().CreateTurnData(turn + 2, _playerRef.id);
                    string msg = JsonConvert.SerializeObject(turnData);
                    _cl.SendMessage(msg);
                    turn++;
                }
                else if (s == "0")
                {
                    _playerRef.id = 0;
                }
                else if (s == "1")
                {
                    _playerRef.id = 1;
                }
                else
                {

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
            CreatePlayer();
        }
    }

    public void CreateClientAndPlayer()
    {
        if (_cl == null)
        {
            host = false;
            _cl = gameObject.AddComponent<Client>();
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
