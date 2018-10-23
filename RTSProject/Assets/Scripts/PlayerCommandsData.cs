using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommandsData : MonoBehaviour
{
    public int playerID;
    public List<MoveCommand> moveCommandList = new List<MoveCommand>();

    public PlayerCommandsData(int id, MoveCommand mc)
    {
        playerID = id;
        moveCommandList.Add(mc);
    }
}