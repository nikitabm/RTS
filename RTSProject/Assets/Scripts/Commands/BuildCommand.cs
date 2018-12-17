using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildCommand : Command
{
    //FIXME: In all command classes change access  modifiers to protected or private(in base class to protected, in other classes to private)
    public int action;
    public BuildCommand(int t, List<int> pUnits, Vector3 pPosition)
    {
        action = t;
        units = pUnits;
        position = pPosition;
    }
    public override void PassSelfToUnit()
    {
        //TODO:
    }
}
