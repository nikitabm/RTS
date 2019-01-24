using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackCommand : Command
{
    [SerializeField]
    public Vector3 position;
    [SerializeField]
    public List<int> units;

    public AttackCommand(List<int> pUnits, Vector3 pPos)
    {
        units = pUnits;
        position = pPos;
    }


    public override void Execute()
    {
        ServiceLocator.GetService<CommandManager>().CommandExecute(this);
    }
}
