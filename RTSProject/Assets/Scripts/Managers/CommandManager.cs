using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class CommandManager : MonoBehaviour, Service
{
    public Queue<Command> _commandQueue = new Queue<Command>();
    public List<Command> _allCommands = new List<Command>();
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
        if (_gm.movementWithoutNetwork) _allCommands.Add(c);
    }

    public void SubsribeToEvent()
    {
        PlayerController.CommandCreated += AddToQueue;
    }

    public void PassCommandsToUnits()
    {

        for (int i = 0; i < _allCommands.Count; i++)
        {
            _allCommands[i].Execute();
            //if (_allCommands[i].GetType() == typeof(MoveCommand))
            //{
            //    if ((_allCommands[i] as MoveCommand).units != null && (_allCommands[i] as MoveCommand).position != Vector3.zero)
            //    {
            //        for (int j = 0; j < (_allCommands[i] as MoveCommand).units.Count; j++)
            //        {
            //            _gm.GetUnit((_allCommands[i] as MoveCommand).units[j]).SetCommand(_allCommands[i]);
            //        }
            //    }
            //}
        }
        _allCommands.Clear();
        OnCommandExecute();
    }
    //public void SendCommand<T>(T pCommand) where T:Command
    //{

    //}

    public PlayerCommandsData CreateTurnData(int turn, int playerID)
    {
        PlayerCommandsData playerData = new PlayerCommandsData(turn, playerID);
        playerData.AddCommand(new MoveCommand(new List<int> { }, Vector3.zero));
        while (_commandQueue.Count != 0)
        {
            _allCommands.Add(_commandQueue.Peek());
            playerData.AddCommand(_commandQueue.Dequeue());
        }

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

    #endregion

}
