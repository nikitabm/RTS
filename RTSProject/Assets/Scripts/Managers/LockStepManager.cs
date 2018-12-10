using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Security;
using UnityEngine.UI;


public class LockStepManager : MonoBehaviour, Service
{
    bool approvedCommands;
    string s;
    List<int> emptyIntList = new List<int>();
    CustomMoveCommand inputCommand;

    //contains key which is number of turn commands were issued and
    //player commands data, which is id of player and list of issued commands
    public PlayerCommandsData commandToSend;

    //contains key- number of turn, and all players commands for this specific turn to execute
    public AllPlayersCommandsData playersmoveData;
    public Client client;
    public delegate void OnNextTurn();
    public static event OnNextTurn NextTurn;



    //private

    //game related variables
    private bool _gameStarted;
    private float _accumilatedTime = 0f;
    private int turn;
    private float _frameLength = 1.0f; //50 ms


    private void Awake()
    {

    }
    void Start()
    {
        //TODO:  move this logic to server (instantiate by server and etc etc)

        playersmoveData = new AllPlayersCommandsData();
        s = "";
        ServiceLocator.ProvideService(this);

        turn = -2;
        //TurnDataToSend = new Dictionary<int, PlayerCommandsData>();
        approvedCommands = false;
    }
    public void SetInputCommand(CustomMoveCommand cm)
    {
        inputCommand = cm;
    }
    public void StartGame()
    {
        _gameStarted = true;
    }
    public void Update()
    {
        if (!_gameStarted) return;
        // WriteTurnData();

        //Basically same logic as FixedUpdate, but we can scale it by adjusting FrameLength
        _accumilatedTime = _accumilatedTime + Time.deltaTime;

        //in case the FPS is too slow, we may need to update the game multiple times a frame
        while (_accumilatedTime > _frameLength)
        {
            print("Turn: " + turn);
            // turn++;
            NextTurn();
            //server send clients "send me data and update" command
            // ServiceLocator.GetService<NetworkingManager>().Send

            _accumilatedTime = _accumilatedTime - _frameLength;

            #region old
            // if (client != null && client._clientState == Client.ClientState.Playing)
            // {
            //     turn++;
            //     print("turn: " + turn);

            // }
            // SendTurnData();
            // //GameFrameTurn();
            // if (client != null && client.host)
            // {
            //     if (client.otherPlayerDataReceived && client.myDataConfirmed)
            //     {
            //         print("getting here?");
            //         client.SendDataToClient("3");
            //         client.readyToTurnWheel = true;
            //         client.myDataConfirmed = false;
            //         client.otherPlayerDataReceived = false;
            //     }
            // }
            // if (client != null && client.readyToTurnWheel)
            // {
            //     print("conformation of turn :" + (turn + 2));
            //     turn = turn + 2;
            //     turnText.text = turn.ToString();
            //     client.readyToTurnWheel = false;
            // }
            #endregion
        }
    }


    // private bool NextTurn()
    // {
    //     if (confirmedActions.ReadyForNextTurn() && pendingActions.ReadyForNextTurn())
    //     {
    //         //increment the turn ID
    //         LockStepTurnID++;
    //         //move the confirmed actions to next turn
    //         confirmedActions.NextTurn();
    //         //move the pending actions to this turn
    //         pendingActions.NextTurn();

    //         return true;
    //     }

    //     return false;
    // }

    public void IncrementTurn()
    {
        if (approvedCommands)
        {
            turn++;
            ExecuteCommands();
        }
    }



    public void ExecuteCommands()
    {
        //do commands
        List<CustomMoveCommand> hostCommands = new List<CustomMoveCommand>();
        List<CustomMoveCommand> clientCommands = new List<CustomMoveCommand>();

        hostCommands = playersmoveData.GetCommandsAtTurn(true, turn);
        clientCommands = playersmoveData.GetCommandsAtTurn(false, turn);
        for (int i = 0; i < hostCommands.Count; i++)
        {
            // hostCommands[i].units
        }

    }

    public void TestListenToCommands()
    {

        // if (pc._selectedObj != null && pc.ObjectSelector.playerState == PlayerController.StateOfPlayer.SelectedLocation)
        // {
        //     unitsSelected.Add(pc._selectedObj.GetComponent<Unit>().ID);


        //     CustomMoveCommand moveCommand = new CustomMoveCommand(unitsSelected, pc._clickPosition);

        //     AllPlayersTurns.Add(turn, new AllPlayersCommandsData(playerID, moveCommand));

        //     commandToSend = new PlayerCommandsData(0, turn, playerID, unitsSelected, pc._clickPosition);


        //     s = JsonUtility.ToJson(commandToSend);

        //     //send to server
        //     (ServiceLocator.GetService(typeof(NetworkingManager)) as NetworkingManager).GetOwningTCPClient().SendMessage(s);

        //     // pc.ObjectSelector.playerState = PlayerController.StateOfPlayer.Idle;
        //     commandToSend = null;
        //     pc._selectedObj = null;
        // }

    }


    public void WriteTurnData()
    {

        // if (inputCommand != null)
        // {
        //     // bool host = (ServiceLocator.GetService(typeof(NetworkingManager)) as NetworkingManager).HasAuthority();
        //     //AllPlayersTurns.Add(turn, new AllPlayersCommandsData(playerID, inputCommand));
        //     inputCommand.turn = turn;
        //     // playersmoveData.RegisterCommand(host, inputCommand);
        //     commandToSend = new PlayerCommandsData(0, turn + 2, playerID, inputCommand.units, inputCommand.pos);
        // }
        // else
        // {
        //     commandToSend = new PlayerCommandsData(-1, turn + 2, playerID, emptyIntList, Vector3.zero);
        // }



    }
    public void SendTurnData()
    {
        //sending turn data to ANOTHER Client
        if (client != null && client._clientState == Client.ClientState.Playing)
        {

            s = JsonUtility.ToJson(commandToSend);
            if (client.host)
            {
                print("Sending turns as HOST");
                // client.SendDataToClient(s);
            }
            else
            {
                print("Sending turns as CLIENT");
                client.SendMessage(s);
            }
            client._turnState = Client.TurnState.WaitingForOtherPlayerDataAndConformation;
            inputCommand = null;
        }
    }

    private void GameFrameTurn()
    {


    }
}
