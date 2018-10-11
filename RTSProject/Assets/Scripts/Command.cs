using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Command : MonoBehaviour
{
    public enum ECommandType
    {
        CM_INVALID,
        // CM_ATTACK,
        CM_MOVE
    };
    public ECommandType command = ECommandType.CM_INVALID;
    public int PlayerID = 0;
    public int NetworkID = 0;
    public virtual void ProcessCommand()
    {

    }

}
public class MoveCommand : Command
{

	void Start()
	{

	}
	public override void ProcessCommand()
	{
		
	}
}