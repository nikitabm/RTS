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
using Newtonsoft.Json;

public class NetworkingManager : MonoBehaviour, Service
{


    public Text serverText;
    public Text ClientText;
    public Text turnText;
    public int turn;

    //FIXME: change the way commands are processed
    public PlayerCommandsData playerOne;
    PlayerCommandsData playerTwo;
    public PlayerCommandsData newData;
    PlayerCommandsData turnData;


    private Client _cl;
    private Server _sr;
    private bool host = false;
    private Player _playerRef;

    void Start()
    {
        turn = -2;
        turnText.text = turn.ToString();
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
                //TODO: 
                //save commands
                if (playerData != null)
                    for (int i = 0; i < playerData.commands.Count; i++)
                    {
                        ServiceLocator.GetService<CommandManager>()._allCommands.Add(playerData.commands[i]);
                    }


            }
            else
            {
                //TODO: important thing to make it more smart and not shit code in here
                if (s == "inc")
                {
                    turnData = ServiceLocator.GetService<CommandManager>().CreateTurnData(turn + 2, _cl.id);
                    string msg = JsonConvert.SerializeObject(turnData);

                    //FIXME: should it be here? should I be here..?
                    //make event for it maybe
                    _cl.SendMessage(msg);
                    turn++;
                }
            }
        }
    }

    private void Update()
    {
        turnText.text = turn.ToString();
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
            _cl.id = 0;
            CreatePlayer();
        }

    }
    public void ConnectToGame()
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
    #endregion
}
