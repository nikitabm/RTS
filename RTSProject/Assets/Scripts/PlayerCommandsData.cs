using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCommandsData
{

    public int playerID;
    public List<int> units = new List<int>();
    public Vector3 pos;
    public int command;
    public List<Command> commands = new List<Command>();
    private int turn;

    public PlayerCommandsData(int pCommand, int pTurn, int pPlayerID, List<int> pUnits, Vector3 pPos)
    {
        command = pCommand;
        units = pUnits;
        pos = pPos;
        playerID = pPlayerID;

    }
    public PlayerCommandsData(int pTurn, int pPlayerID, Command c)
    {
        turn = pTurn;
        playerID = pPlayerID;
        commands.Add(c);
    }
    public PlayerCommandsData(int pTurn, int pPlayerID)
    {
        turn = pTurn;
        playerID = pPlayerID;

    }
    public void AddCommand(Command c)
    {
        commands.Add(c);
    }
}