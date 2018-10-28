using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommandsData
{

    public int turn;
    public int playerID;
    public List<int> units = new List<int>();
    public Vector3 pos;
    public CustomMoveCommand moveCommand = new CustomMoveCommand();

    public PlayerCommandsData(int pTurn, int pPlayerID, int pUnitID, Vector3 pPos)
    {
        turn = pTurn;
        moveCommand.pos = pos;
        moveCommand.units = units;
        // playerID = pPlayerID;
        // units.Add(pUnitID);
        // pos=pPos;
    }
    public PlayerCommandsData(int pTurn,int pPlayerID, CustomMoveCommand pCommand)
    {
        turn = pTurn;
        playerID=pPlayerID;
        moveCommand = pCommand;
    }
}