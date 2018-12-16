using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CommandManager : MonoBehaviour, Service
{
    public Queue<Command> _commandQueue = new Queue<Command>();
    void Start()
    {
        //responsible for queing commands and operating with them
        ServiceLocator.ProvideService(this);
    }
    public void AddToQueue(Command c)
    {
        _commandQueue.Enqueue(c);
        print("new Command: " + c.position + " ");
        print("units count: " + c.units.Count + " unit[0]: " + c.units[0]);
    }
    public void DequequeCommand()
    {
        print(_commandQueue.Peek().position);
        print("dequeued units count: " + _commandQueue.Peek().units.Count + " unit[0]: " + _commandQueue.Peek().units[0]);
        _commandQueue.Dequeue();
    }
    public void SubsribeToEvent()
    {
        SelectObject.commandCreated += AddToQueue;
    }
    public void ExecuteCommand(PlayerCommandsData commandData)
    {
        //TODO:
        for (int i = 0; i < commandData.commands.Count; i++)
        {
            commandData.commands[i].Execute();
        }
    }
    public PlayerCommandsData CreateTurnData(int turn, int playerID)
    {
        PlayerCommandsData playerData;
        playerData = new PlayerCommandsData(turn, playerID);
        if (_commandQueue.Count == 0) playerData.AddCommand(Command.CreateCommand<EmptyCommand>());
        while (_commandQueue.Count != 0)
        {
            playerData.AddCommand(_commandQueue.Dequeue());
        }
        print(JsonUtility.ToJson(playerData));
        return playerData;
    }
}
