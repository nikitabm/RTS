using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackCommand : Command
{
    public Vector3 position;
    public List<int> units;

    public AttackCommand(List<int> pUnits, Vector3 pPos)
    {
        units = pUnits;
        position = pPos;
    }


    public override void Execute()
    {
        Command.CommandExecute(this);
    }
}
