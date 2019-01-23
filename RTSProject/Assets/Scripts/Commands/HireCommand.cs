using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HireCommand : Command
{

    public HireCommand()
    {

    }

    public override void Execute()
    {
        ServiceLocator.GetService<CommandManager>().CommandExecute(this);

    }
}
