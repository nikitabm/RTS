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

    //contains key which is number of turn commands were issued and
    //player commands data, which is id of player and list of issued commands
    public PlayerCommandsData commandToSend;

    //contains key- number of turn, and all players commands for this specific turn to execute
    public Dictionary<int, AllPlayersCommandsData> AllPlayersTurns = new Dictionary<int, AllPlayersCommandsData>();

    //public
    LockStepManager Instance;

    //private
    private float AccumilatedTime = 0f;

    private float FrameLength = 0.05f; //50 ms


    private void Awake()
    {
        ServiceLocator.ProvideService(this);
        Instance = this;
        turn = 0;
        //TurnDataToSend = new Dictionary<int, PlayerCommandsData>();
        AllPlayersTurns = new Dictionary<int, AllPlayersCommandsData>();
        approvedCommands = false;
    }
    void Start()
    {
        pc = (ServiceLocator.GetService(typeof(GameManager)) as GameManager).TeamOneController.GetComponent<PlayerController>();
    }
    public void Update()
    {
        //Basically same logic as FixedUpdate, but we can scale it by adjusting FrameLength
        AccumilatedTime = AccumilatedTime + Time.deltaTime;

        //in case the FPS is too slow, we may need to update the game multiple times a frame
        while (AccumilatedTime > FrameLength)
        {
            //testing
            TestListenToCommands();



            // WriteTurnData();
            // SendTurnData();
            // GameFrameTurn();

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
    }

    public void TestListenToCommands()
    {

        if (pc._selectedObj != null && pc.ObjectSelector.playerState == PlayerController.StateOfPlayer.SelectedLocation)
        {
            print(pc._selectedObj.GetComponent<Unit>().ID);
            List<int> unitsSelected = new List<int>();
            unitsSelected.Add(pc._selectedObj.GetComponent<Unit>().ID);
            unitsSelected.Add(10);
            unitsSelected.Add(11);
            unitsSelected.Add(12);


            CustomMoveCommand moveCommand = new CustomMoveCommand(unitsSelected, pc._clickPosition);
            print(moveCommand.units.Count + " " + moveCommand.pos);

            AllPlayersTurns.Add(turn, new AllPlayersCommandsData(playerID, moveCommand));
            print("Turn: " + turn);

            string s = "";
            string t = "";

            //{ "turn":0,"playerID":0, {"units":[0],"pos":{"x":6.5235595703125,"y":-1.3772476548898187e-18,"z":-13.682971954345704}}
            commandToSend = new PlayerCommandsData("",turn, playerID, unitsSelected,pc._clickPosition);
            // t = JsonUtility.ToJson(moveCommand);
            // t.Remove(0, 1);
            // t.Replace("{ ", ", ");
            // s.Replace("}}", "},");
            // s = "{ \"turn\":" + turn + ",\"playerID\":" + playerID + ", " + "\"MoveCommand\":" + t;
            // print(s);
            s=JsonUtility.ToJson(commandToSend);
            
            //send to server
            (ServiceLocator.GetService(typeof(NetworkingManager)) as NetworkingManager).GetOwningTCPClient().SendMessage(s);

            pc.ObjectSelector.playerState = PlayerController.StateOfPlayer.Idle;
            turn++;
            commandToSend = null;
            pc._selectedObj = null;
        }

    }
    public void WriteTurnData()
    {
        MoveCommand moveCommand = new MoveCommand();
        if (pc._selectedObj != null && pc.ObjectSelector.playerState == PlayerController.StateOfPlayer.SelectedLocation)
        {
            moveCommand._unit = pc._selectedObj;
            moveCommand._position = pc._clickPosition;
            moveCommand._position.w = pc._selectedObj.GetComponent<Unit>().ID;
            pc._clickPosition = Vector3.zero;
            pc._selectedObj = null;
            pc.ObjectSelector.playerState = PlayerController.StateOfPlayer.Idle;
        }
        //queues commands to dictionaries.
        // PlayerCommandsData data = new PlayerCommandsData(playerID, moveCommand);
        // TurnDataToSend.Add(turn, data);
        // AllPlayersTurns.Add(turn, new AllPlayersCommandsData(playerID, moveCommand));
        // pc._commandList.Add(moveCommand);
        // print("Turn: " + turn);
        // print("Move command: " + moveCommand._position + " " + moveCommand._unit);
    }
    public void SendTurnData()
    {
        // (ServiceLocator.GetService(typeof(NetworkingManager)) as NetworkingManager).
        // GetOwningTCPClient().SendCommand(turn,TurnDataToSend[turn]);
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
