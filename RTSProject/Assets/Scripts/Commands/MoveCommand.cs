using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveCommand : Command
{

    public MoveCommand(int id, List<int> pUnits, Vector3 pPosition)
    {
        _units = pUnits;
        _position = pPosition;
    }
}
