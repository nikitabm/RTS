using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomMoveCommand
{
    public int turn;
    public int playerID;
    public List<int> units = new List<int>();
    public Vector3 pos;
    public CustomMoveCommand(List<int> pUnits, Vector3 pPos, int pTurn)
    {
        turn = pTurn;
        units = pUnits;
        pos = pPos;
    }
    public CustomMoveCommand(List<int> pUnits, Vector3 pPos)
    {
        units = pUnits;
        pos = pPos;
    }
    public CustomMoveCommand()
    {

    }

}
