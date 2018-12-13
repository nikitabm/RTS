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
    }
    public void SubsribeToEvent()
    {
        SelectObject.commandCreated += AddToQueue;
    }
    public void ExecuteCommand(PlayerCommandsData commandData)
    {
        //TODO:
        // for (int i = 0; i < commandData.commands.Count; i++)
        // {
        //     if (ServiceLocator.GetService<GameManager>().getUnit(commandData.commands[i]._units[0]) != null)
        //     {
        //         NavMeshAgent agent = ServiceLocator.GetService<GameManager>().getUnit(commandData.commands[i]._units[0]).GetComponent<Unit>().GetComponent<NavMeshAgent>();
        //         agent.destination = commandData.commands[i]._position;
        //     }
        // }
    }
    public PlayerCommandsData CreateTurnData(int turn, int playerID)
    {
        PlayerCommandsData playerData;
        playerData = new PlayerCommandsData(turn, playerID);

        if (_commandQueue.Count == 0) playerData.AddCommand(new Command(playerID, new List<int> { -1 }, Vector3.zero));
        while (_commandQueue.Count != 0)
        {
            playerData.AddCommand(_commandQueue.Dequeue());
        }
        return playerData;
    }
}
