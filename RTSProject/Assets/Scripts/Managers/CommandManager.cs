using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class CommandManager : MonoBehaviour, Service
{
    public Queue<Command> _commandQueue = new Queue<Command>();
    public List<Command> _allCommands;
    // public Queue<Command> _allCommands = new Queue<Command>();

    void Start()
    {
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
    public void PassCommandsToUnits()
    {
        for (int i = 0; i < _allCommands.Count; i++)
        {

        }
    }
    public PlayerCommandsData CreateTurnData(int turn, int playerID)
    {
        PlayerCommandsData playerData = new PlayerCommandsData(turn, playerID);
        if (_commandQueue.Count == 0) playerData.AddCommand(Command.CreateCommand<EmptyCommand>());
        while (_commandQueue.Count != 0)
        {
            _allCommands.Add(_commandQueue.Peek());
            playerData.AddCommand(_commandQueue.Dequeue());
        }

        return playerData;
    }
}
