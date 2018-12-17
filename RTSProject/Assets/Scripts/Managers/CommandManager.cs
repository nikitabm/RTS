using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
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
        PlayerCommandsData playerData = new PlayerCommandsData(turn, playerID);
        if (_commandQueue.Count == 0) playerData.AddCommand(Command.CreateCommand<EmptyCommand>());
        while (_commandQueue.Count != 0)
        {
            playerData.AddCommand(_commandQueue.Dequeue());
        }
        return playerData;
    }
}
