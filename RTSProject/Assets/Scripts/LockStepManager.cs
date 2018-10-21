using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerCommandsData : MonoBehaviour
{
    public int playerID;
    public List<MoveCommand> moveCommandList = new List<MoveCommand>();

    public PlayerCommandsData(int id, MoveCommand mc)
    {
        playerID = id;
        moveCommandList.Add(mc);
    }
}

public class AllPlayersCommandsData : MonoBehaviour
{
    public List<MoveCommand> moveCommandListHost = new List<MoveCommand>();

    public List<MoveCommand> moveCommandListClient = new List<MoveCommand>();


    public AllPlayersCommandsData(int ID, MoveCommand mc)
    {
        if (ID == 0) moveCommandListHost.Add(mc);
        if (ID == 1) moveCommandListClient.Add(mc);

    }

}
public class LockStepManager : MonoBehaviour
{

    PlayerController pc;
    int turn;
    int playerID;

    //contains key which is number of turn commands were issued and
    //player commands data, which is id of player and list of issued commands
    public Dictionary<int, PlayerCommandsData> TurnDataToSend = new Dictionary<int, PlayerCommandsData>();

    //contains key- number of turn, and all players commands for this specific turn to execute
    public Dictionary<int, AllPlayersCommandsData> AllPlayersTurns = new Dictionary<int, AllPlayersCommandsData>();

    //public
    LockStepManager Instance;

    //private
    private float AccumilatedTime = 0f;

    private float FrameLength = 0.05f; //50 ms


    private void Awake()
    {
        Instance = this;
        turn = 0;
        TurnDataToSend = new Dictionary<int, PlayerCommandsData>();
        AllPlayersTurns = new Dictionary<int, AllPlayersCommandsData>();
        pc = (ServiceLocator.GetService(typeof(NetworkingManager))) as PlayerController;
    }
    void Start()
    {

    }
    public void Update()
    {
        //Basically same logic as FixedUpdate, but we can scale it by adjusting FrameLength
        AccumilatedTime = AccumilatedTime + Time.deltaTime;

        //in case the FPS is too slow, we may need to update the game multiple times a frame
        while (AccumilatedTime > FrameLength)
        {
            GameFrameTurn();
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


    public  void WriteTurnData()
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
        TurnDataToSend.Add(turn, new PlayerCommandsData(playerID, moveCommand));
        AllPlayersTurns.Add(turn, new AllPlayersCommandsData(playerID, moveCommand));
        pc._commandList.Add(moveCommand);
        print("Turn: "+turn);
        print("Move command: "+moveCommand._position + " " + moveCommand._unit);
    }
    public void SendTurnData()
    {
        (ServiceLocator.GetService(typeof(NetworkingManager)) as NetworkingManager).
        GetOwningTCPClient().SendCommand(turn,TurnDataToSend[turn]);
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
