using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackCommand : Command
{
    public AttackCommand(List<int> pUnits, Vector3 pPos)
    {
        units = pUnits;
        position = pPos;
    }
    public override void Execute()
    {
        //TODO:
    }    

}
