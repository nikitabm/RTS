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
        ServiceLocator.GetService<CommandManager>().CommandExecute(this);

    }
    public void AssignCommand()
    {
        for (int i = 0; i < units.Count; i++)
        {
            ServiceLocator.GetService<GameManager>().GetUnit(units[i]).SetCommand(this);
        }
    }
}
