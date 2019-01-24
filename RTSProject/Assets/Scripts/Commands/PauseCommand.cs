using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PauseCommand : Command
{

    public PauseCommand()
    {

    }

    public override void Execute()
    {
        ServiceLocator.GetService<CommandManager>().CommandExecute(this);
    }
}
