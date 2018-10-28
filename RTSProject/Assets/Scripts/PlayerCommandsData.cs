using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommandsData
{

    public int turn;
    public int playerID;
    public List<int> units = new List<int>();
    public Vector3 pos;
    public CustomMoveCommand moveCommand;

    public PlayerCommandsData(int pTurn,int pPlayerID, CustomMoveCommand pCommand)
    {
        moveCommand=new CustomMoveCommand(pCommand.units,pCommand.pos);
        turn = pTurn;
        playerID=pPlayerID;
        
    }
}