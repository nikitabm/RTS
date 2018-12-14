using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCommand : Command
{
    int action;
    public BuildCommand(int t, List<int> pUnits, Vector3 pPosition)
    {
        action = t;
        units = pUnits;
        position = pPosition;
    }
    public override void Execute()
    {

    }
}
