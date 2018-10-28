using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPlayersCommandsData
{
    public List<CustomMoveCommand> moveCommandListHost = new List<CustomMoveCommand>();

    public List<CustomMoveCommand> moveCommandListClient = new List<CustomMoveCommand>();


    public AllPlayersCommandsData(int PlayerID, CustomMoveCommand mc)
    {
        if (PlayerID == 0) moveCommandListHost.Add(mc);
        if (PlayerID == 1) moveCommandListClient.Add(mc);

    }

}