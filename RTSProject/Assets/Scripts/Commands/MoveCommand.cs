using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveCommand : Command
{

    public MoveCommand(List<int> pUnits, Vector3 pPosition)
    {
        units = pUnits;
        position = pPosition;
    }
    public override void Execute()
    {
        var gm = ServiceLocator.GetService<GameManager>();
        var _unit = gm.GetUnit(units[0]);
    }
}
