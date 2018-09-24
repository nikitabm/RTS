using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Command : MonoBehaviour
{

    public enum ECommandType
    {
        CM_INVALID,
        CM_ATTACK,
        CM_MOVE
    };
    public virtual void ProcessCommand()
	{

	}

}
public class SelectCommand : Command
{
	public override void ProcessCommand()
	{

	}

}