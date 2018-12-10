using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour, Service
{


    public Queue<Command> _commandQueue = new Queue<Command>();
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
        _commandQueue.Enqueue(c);
        print((c as MoveCommand)._position + " " + (c as MoveCommand)._units);
        print(_commandQueue.Count);
    }
    public void SubsribeToEvent()
    {
        SelectObject.commandCreated += AddToQueue;
    }
    public PlayerCommandsData CreateTurnData(Command pCommand, int turn, int playerID)
    {
        // int turn = ServiceLocator.GetService<NetworkingManager>().turn;
        // int playerID = ServiceLocator.GetService<GameManager>().GetPlayer().id;
        // if()
        PlayerCommandsData playerData;
        playerData = new PlayerCommandsData(turn, playerID, pCommand);
        while (_commandQueue.Count != 0)
            playerData.AddCommand(_commandQueue.Dequeue());
        return playerData;
    }
    public PlayerCommandsData CreateTurnData(int turn, int playerID)
    {
        // int turn = ServiceLocator.GetService<NetworkingManager>().turn;
        // int playerID = ServiceLocator.GetService<GameManager>().GetPlayer().id;

        PlayerCommandsData playerData;
        playerData = new PlayerCommandsData(turn, playerID);
        while (_commandQueue.Count != 0)
            playerData.AddCommand(_commandQueue.Dequeue());
        return playerData;
    }
}
