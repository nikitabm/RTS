﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCommandsData
{

    public int PlayerID { get; set; }

    public int Turn { get; set; }

    public List<Command> commands { get; set; }

    public PlayerCommandsData(int pTurn, int pPlayerID)
    {
        commands = new List<Command>();
        Turn = pTurn;
        PlayerID = pPlayerID;

    }
    public void AddCommand(Command c)
    {
        commands.Add(c);
    }
}