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
        _commandQueue.Enqueue(c);
        // _commandList.Add(c);
        // print((c as MoveCommand)._position + " " + (c as MoveCommand)._units[0]);
        // print(_commandQueue.Count);
        // print(JsonUtility.ToJson(c));
    }
    public void SubsribeToEvent()
    {
        SelectObject.commandCreated += AddToQueue;
    }
    public void ExecuteCommand(PlayerCommandsData commandData)
    {
        for (int i = 0; i < commandData.commands.Count; i++)
        {
            ServiceLocator.GetService<GameManager>().getUnit(commandData.commands[i]._units[0]);
            
        }
    }
    public PlayerCommandsData CreateTurnData(int turn, int playerID)
    {
        PlayerCommandsData playerData;
        playerData = new PlayerCommandsData(turn, playerID);

        if (_commandQueue.Count == 0) playerData.AddCommand(new Command(playerID, new List<int> { -1 }, Vector3.zero));
        while (_commandQueue.Count != 0)
        {
            playerData.AddCommand(_commandQueue.Dequeue());
            // coms += JsonUtility.ToJson(_commandQueue.Dequeue()) + ",";
            //playerData.AddCommand(_commandQueue.Dequeue());
        }
        // print(coms);
        return playerData;
    }
}
