using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPlayersCommandsData : MonoBehaviour
{
    public List<MoveCommand> moveCommandListHost = new List<MoveCommand>();

    public List<MoveCommand> moveCommandListClient = new List<MoveCommand>();


    public AllPlayersCommandsData(int ID, MoveCommand mc)
    {
        if (ID == 0) moveCommandListHost.Add(mc);
        if (ID == 1) moveCommandListClient.Add(mc);

    }

}