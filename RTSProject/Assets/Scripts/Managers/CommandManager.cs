using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class CommandManager : MonoBehaviour, Service
{
    public Queue<Command> _commandQueue = new Queue<Command>();
    public List<Command> _allCommands;
    private GameManager _gm;
    public delegate void ExecuteCommand();
    public static ExecuteCommand OnCommandExecute;
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
        _allCommands.Add(c);
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
                for (int j = 0; j < _allCommands[i].units.Count; j++)
                {
                    _gm.GetUnit(_allCommands[i].units[j]).CurrentCommand = _allCommands[i];
                }
        }
        OnCommandExecute();
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
