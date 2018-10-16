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
    public Vector4 _position;
    public GameObject _unit;
    public MoveCommand()
    {
        // _position=pos;
        // _unit=obj;
    }
	void Start()
	{

	}
	public override void ProcessCommand()
	{
		
	}
}