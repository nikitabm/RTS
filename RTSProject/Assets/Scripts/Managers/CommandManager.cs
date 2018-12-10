using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour, Service
{


    public Queue<Command> _commandQueue = new Queue<Command>();
    public List<Command> _commandList = new List<Command>();
    void Start()
    {
        ServiceLocator.ProvideService(this);
        //responsible for queuing commands and operating with them

    }

    void Update()
    {

    }
    public void AddToQueue(Command c)
    {
        // _commandQueue.Enqueue(c);
        _commandList.Add(c);
        print(_commandList[_commandList.Count - 1]._units[0]);
        print(JsonUtility.ToJson(_commandList[_commandList.Count - 1]));
        print("Testing");
        // print((c as MoveCommand)._position + " " + (c as MoveCommand)._units[0]);
        // print(_commandQueue.Count);
        // print(JsonUtility.ToJson(c));
    }
    public void SubsribeToEvent()
    {
        SelectObject.commandCreated += AddToQueue;
    }
    // public PlayerCommandsData CreateTurnData(Command pCommand, int turn, int playerID)
    // {
    //     // int turn = ServiceLocator.GetService<NetworkingManager>().turn;
    //     // int playerID = ServiceLocator.GetService<GameManager>().GetPlayer().id;
    //     // if()
    //     PlayerCommandsData playerData;
    //     playerData = new PlayerCommandsData(turn, playerID, pCommand);
    //     while (_commandQueue.Count != 0)
    //         playerData.AddCommand(_commandQueue.Dequeue());
    //     return playerData;
    // }
    public PlayerCommandsData CreateTurnData(int turn, int playerID)
    {
        // int turn = ServiceLocator.GetService<NetworkingManager>().turn;
        // int playerID = ServiceLocator.GetService<GameManager>().GetPlayer().id;
        // string coms = "";
        // PlayerCommandsData playerData;
        // playerData = new PlayerCommandsData(turn, playerID);
        for (int i = 0; i < _commandList.Count; i++)
        {
            print(_commandList[i]._units[0]);
        }



        // while (_commandQueue.Count != 0)
        // {
        //     coms += JsonUtility.ToJson(_commandQueue.Dequeue()) + ",";
        //     //playerData.AddCommand(_commandQueue.Dequeue());
        //     //{"playerID":0,"NetworkID":0,"_position":{"x":2.9861931800842287,"y":8.448469405058942e-17,"z":-3.80485200881958},"_units":[2]},
        //     //{"playerID":0,"NetworkID":0,"_position":{"x":2.878112316131592,"y":7.593503464858013e-17,"z":-3.4198098182678224},"_units":[2]},
        // }
        // print("all commands");
        // print(coms);
        return null;
    }
}
