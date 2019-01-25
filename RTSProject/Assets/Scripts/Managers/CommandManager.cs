using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
public class CommandManager : MonoBehaviour, Service
{
    public static List<Type> commandTypes;
    public Queue<Command> _commandQueue = new Queue<Command>();
    public List<Command> allCommands = new List<Command>();
    private GameManager _gm;
    public delegate void ExecuteCommand();
    public static ExecuteCommand OnCommandExecute;
    public delegate void OnPause();
    public static OnPause GamePause;
    // public Queue<Command> _allCommands = new Queue<Command>();

    private void Awake()
    {
        commandTypes = new List<Type>
        {
            typeof(AttackCommand),
            typeof(BuildCommand),
            typeof(HireCommand),
            typeof(PauseCommand),
            typeof(MoveCommand),
            typeof(EmptyCommand)
        };
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
        if (_gm.movementWithoutNetwork) allCommands.Add(c);
    }

    public void SubsribeToEvent()
    {
        PlayerController.CommandCreated += AddToQueue;
    }

    public void PassCommandsToUnits()
    {

        for (int i = 0; i < allCommands.Count; i++)
        {
            allCommands[i].Execute();

        }
        allCommands.Clear();
        //OnCommandExecute();
    }

    public PlayerCommandsData CreateTurnData(int turn, int playerID)
    {
        PlayerCommandsData playerData = new PlayerCommandsData(turn, playerID);
        while (_commandQueue.Count != 0)
        {
            allCommands.Add(_commandQueue.Peek());
            playerData.AddCommand(_commandQueue.Dequeue());
        }
        //playerData.AddCommand(new BuildCommand(5, new List<int> { 1, 2, 3 }, Vector3.zero));
        return playerData;
    }
    #region Commands Actions
    public void CommandExecute(Command c)
    {
        Debug.Log("Command execute is called for base class");
    }

    public void CommandExecute(MoveCommand c)
    {
        c.AssignCommand();
        //Debug.Log(c.ToString());
    }

    public void CommandExecute(EmptyCommand c)
    {
        Debug.Log(c.ToString());
    }

    public void CommandExecute(BuildCommand c)
    {
        Debug.Log(c.ToString());
    }

    public void CommandExecute(AttackCommand c)
    {
        Debug.Log(c.ToString());
    }

    public void CommandExecute(PauseCommand c)
    {
        _gm.GamePaused = !_gm.GamePaused;
        GamePause();
    }

    public void CommandExecute(HireCommand c)
    {
        Debug.Log(c.ToString());
    }

    #endregion

}
