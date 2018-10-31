using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Security;


public class LockStepManager : MonoBehaviour, Service
{

    PlayerController pc;
    int turn;
    int playerID;
    bool approvedCommands;
    string s;
    List<int> emptyIntList = new List<int>();
    CustomMoveCommand inputCommand;

    //contains key which is number of turn commands were issued and
    //player commands data, which is id of player and list of issued commands
    public PlayerCommandsData commandToSend;

    //contains key- number of turn, and all players commands for this specific turn to execute
    public AllPlayersCommandsData playersmoveData;
    public TcpTestClient client;

    //public
    LockStepManager Instance;

    //private
    private float AccumilatedTime = 0f;

    private float FrameLength = 1.0f; //50 ms


    private void Awake()
    {
        playersmoveData = new AllPlayersCommandsData();
        s = "";
        ServiceLocator.ProvideService(this);
        Instance = this;
        turn = -2;
        //TurnDataToSend = new Dictionary<int, PlayerCommandsData>();
        approvedCommands = false;
    }
    void Start()
    {
        pc = (ServiceLocator.GetService(typeof(GameManager)) as GameManager).TeamOneController.GetComponent<PlayerController>();

    }
    public void SetInputCommand(CustomMoveCommand cm)
    {
        inputCommand = cm;
    }
    public void Update()
    {
        WriteTurnData();
        //Basically same logic as FixedUpdate, but we can scale it by adjusting FrameLength
        AccumilatedTime = AccumilatedTime + Time.deltaTime;

        //in case the FPS is too slow, we may need to update the game multiple times a frame
        while (AccumilatedTime > FrameLength)
        {
            //testing
            //TestListenToCommands();


            if (client != null && client._clientState == TcpTestClient.ClientState.Playing)
            {
                turn++;
                print("turn: " + turn);

            }
            SendTurnData();
            //GameFrameTurn();
            if (client.host)
            {
                if (client.otherPlayerDataReceived&&client.myDataConfirmed)
                {
                    client.SendDataToClient("3");
                    client.readyToTurnWheel=true;
                    client.myDataConfirmed=false;
                    client.otherPlayerDataReceived=false;
                }
            }
            if(client.readyToTurnWheel)
            {
                print("conformation of turn :"+(turn+2));
            }
            AccumilatedTime = AccumilatedTime - FrameLength;
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

        if (inputCommand != null)
        {
            bool host = (ServiceLocator.GetService(typeof(NetworkingManager)) as NetworkingManager).HasAuthority();
            //AllPlayersTurns.Add(turn, new AllPlayersCommandsData(playerID, inputCommand));
            inputCommand.turn = turn;
            playersmoveData.RegisterCommand(host, inputCommand);
            commandToSend = new PlayerCommandsData(0, turn+2, playerID, inputCommand.units, inputCommand.pos);
        }
        else
        {
            commandToSend = new PlayerCommandsData(-1, turn+2, playerID, emptyIntList, Vector3.zero);
        }



    }
    public void SendTurnData()
    {
        //sending turn data to ANOTHER Client
        if (client != null && client._clientState == TcpTestClient.ClientState.Playing)
        {
            print("Sending turns");
            s = JsonUtility.ToJson(commandToSend);
            if (client.host)
            {
                client.SendDataToClient(s);
            }
            else
            {
                client.SendMessage(s);
            }
            client._turnState = TcpTestClient.TurnState.WaitingForOtherPlayerDataAndConformation;
            inputCommand = null;
        }
    }

    private void GameFrameTurn()
    {
        //first frame is used to process actions
        // if (GameFrame == 0)
        // {
        //     if (LockStepTurn())
        //     {
        //         GameFrame++;
        //     }
        // }
        // else
        // {
        //update game
        // SceneManager.Manager.TwoDPhysics.Update(GameFramesPerSecond);

        // List<IHasGameFrame> finished = new List<IHasGameFrame>();
        // foreach (IHasGameFrame obj in SceneManager.Manager.GameFrameObjects)
        // {
        //     obj.GameFrameTurn(GameFramesPerSecond);
        //     if (obj.Finished)
        //     {
        //         finished.Add(obj);
        //     }
        // }

        // foreach (IHasGameFrame obj in finished)
        // {
        //     SceneManager.Manager.GameFrameObjects.Remove(obj);
        // }

        // GameFrame++;
        // if (GameFrame == GameFramesPerLocksetpTurn)
        // {
        //     GameFrame = 0;
        // }
        // }
    }
}
