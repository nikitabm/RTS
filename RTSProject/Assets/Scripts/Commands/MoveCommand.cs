using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveCommand : Command
{
    public Vector3 position;
    public List<int> units;

    public MoveCommand(List<int> pUnits, Vector3 pPosition)
    {
        units = pUnits;
        position = pPosition;
    }
    public override void Execute()
    {
        Command.ActualExecute(this);
    }
}
