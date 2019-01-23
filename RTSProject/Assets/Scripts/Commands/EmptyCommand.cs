using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EmptyCommand : Command
{

    public EmptyCommand()
    {

    }

    public override void Execute()
    {
        Command.CommandExecute(this);
    }
}
