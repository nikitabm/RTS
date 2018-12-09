﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCommandsData
{

    public int playerID;
    public List<int> units = new List<int>();
    public Vector3 pos;
    public int command;
    private List<MoveCommand> commands = new List<MoveCommand>();
    private int turn;

    public PlayerCommandsData(int s, int pTurn, int pPlayerID, List<int> pUnits, Vector3 pPos)
    {
        command = s;
        units = pUnits;
        pos = pPos;
        playerID = pPlayerID;

    }
}