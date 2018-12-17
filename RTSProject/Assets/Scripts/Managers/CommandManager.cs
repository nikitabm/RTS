using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class CommandManager : MonoBehaviour, Service
{
    public Queue<Command> _commandQueue = new Queue<Command>();
    public List<Command> _allCommands;
    private GameManager _gm;
    // public Queue<Command> _allCommands = new Queue<Command>();
    private void Awake()
    {
        ServiceLocator.ProvideService(this);
    }
    private void Start()
    {
        _gm = ServiceLocator.GetService<GameManager>();
        NetworkingManager.proccessCommands += PassCommandsToUnits;

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
            if (_allCommands[i].units != null)
                _gm.GetUnit(_allCommands[i].units[0]).CurrentCommand = _allCommands[i];
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
