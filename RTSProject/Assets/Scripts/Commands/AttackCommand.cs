using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : Command
{
    // private List<int> _units;
    public AttackCommand(List<int> pUnits, Vector3 pPos)
    {
        _units = pUnits;
        _position = pPos;
    }
    // public override void ProcessCommand()
    // {

    // }
}
