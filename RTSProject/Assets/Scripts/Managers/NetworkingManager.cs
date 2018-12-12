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
    public Text turnText;
    public int turn;
    public PlayerCommandsData playerOne;
    PlayerCommandsData playerTwo;

    public PlayerCommandsData newData;
    PlayerCommandsData turnData;




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
    // private void ClientSend()
    // {
    //     //TODO:
    //     // _cl.send
    //     //client send data to server method
    //     //make it generic
    // }
    // private void ServerSend()
    // {
    //     //TODO:
    //     //same but for server
    // }
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
        PlayerCommandsData playerData = JsonUtility.FromJson<PlayerCommandsData>(s);

        if (playerData.playerID == 0)
        {
            playerOne = playerData;
        }
        else
        {
            playerTwo = playerData;

        }
        if (playerOne != null && playerTwo != null)
        {
            print("send data to players");
            _sr.SendMessageToClients(JsonUtility.ToJson(playerTwo), JsonUtility.ToJson(playerOne));
            playerOne = null;
            playerTwo = null;

        }
    }
    public void DecodeMessage(string s)
    {
        if (_cl != null)
        {
            if (s == "inc")
            {
                //TODO: important thing to make it more smart and not shit code in here
                turnData = ServiceLocator.GetService<CommandManager>().CreateTurnData(turn + 2, _cl.id);
                print("client receives INC command and sends turn data");
                // msg = turnData.commands.Count.ToString();

                string msg = JsonUtility.ToJson(turnData);

                print(msg);


                _cl.SendMessage(msg);
                //should it be here?
                turn++;
            }

            else if (s.Length > 10)
            {
                PlayerCommandsData playerData = JsonUtility.FromJson<PlayerCommandsData>(s);
                print(s);
                ServiceLocator.GetService<CommandManager>().ExecuteCommand(playerData);
                ServiceLocator.GetService<CommandManager>().ExecuteCommand(turnData);

            }
            else
            {
                //FIXME:
                //_cl.id = (int)char.GetNumericValue(s.ToCharArray()[0]);
                print(_cl.id);
            }
        }
    }
    // public static 

    private void Update()
    {
        turnText.text = turn.ToString();
        // if (Input.GetKeyDown(KeyCode.H))
        // {
        //     PlayerCommandsData turnData = ServiceLocator.GetService<CommandManager>().CreateTurnData(turn + 2, _cl.id);
        //     string msg = JsonUtility.ToJson(turnData);
        //     newData = JsonUtility.FromJson<PlayerCommandsData>(msg);
        //     _cl.SendMessage(msg);
        //     print(msg);
        // }
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
    }
    #endregion
}
