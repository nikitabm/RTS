using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class PlayerCommandsData
{

    public int PlayerID;

    public int Turn;
    public List<string> commands;
    public PlayerCommandsData(int pTurn, int pPlayerID)
    {
        commands = new List<string>();
        Turn = pTurn;
        PlayerID = pPlayerID;
    }

    public void AddCommand(Command c)
    {
        commands.Add(ConvertCommandToString(c));
    }

    public string ConvertCommandToString(Command c)
    {
        string s = "";
        s += CommandManager.commandTypes.IndexOf(c.GetType());
        s += JsonConvert.SerializeObject(c);
        return s;
    }
}