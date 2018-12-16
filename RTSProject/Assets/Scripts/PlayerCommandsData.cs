using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCommandsData
{

    public int playerID;

    public int turn;

    public List<Command> commands = new List<Command>();

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