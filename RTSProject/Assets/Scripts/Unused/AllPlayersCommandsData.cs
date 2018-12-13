using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPlayersCommandsData
{
    int turn;
    // private List<CustomMoveCommand> moveCommandListHost = new List<CustomMoveCommand>();

    // private List<CustomMoveCommand> moveCommandListClient = new List<CustomMoveCommand>();



    public AllPlayersCommandsData()
    {
    }
    // public void RegisterCommand(bool host, CustomMoveCommand c)
    // {
    //     if (host) moveCommandListHost.Add(c);
    //     else moveCommandListClient.Add(c);
    // }
    // public List<CustomMoveCommand> GetCommandsAtTurn(bool host, int t)
    // {
    //     List<CustomMoveCommand> temp = new List<CustomMoveCommand>();
    //     if (host)
    //     {
    //         for (int i = 0; i < moveCommandListHost.Count; i++)
    //         {
    //             if (moveCommandListHost[i].turn == t) temp.Add(moveCommandListHost[i]);
    //             else continue;
    //         }
    //     }
    //     else
    //     {
    //         {
    //             for (int i = 0; i < moveCommandListClient.Count; i++)
    //                 if (moveCommandListClient[i].turn == t) temp.Add(moveCommandListClient[i]);
    //                 else continue;
    //         }
    //     }
    //     return temp;
    // }

}