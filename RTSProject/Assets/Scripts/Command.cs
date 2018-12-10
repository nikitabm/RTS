using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Command
{
    public enum ECommandType
    {
        CM_INVALID,
        CM_ATTACK,
        CM_MOVE
    };
    public ECommandType command = ECommandType.CM_INVALID;
    public int PlayerID = 0;
    public int NetworkID = 0;
    public virtual void ProcessCommand()
    {


    }
    // public static Command CreateCommand(List<int> pUnits, Vector3 pos)
    // {

    //     // return new MoveCommand();
    // }
}
public class EmptyCommand : Command
{

}
public class MoveCommand : Command
{
    int playerID;
    public Vector3 _position;
    public List<int> _units;
    public MoveCommand(int id, List<int> pUnits, Vector3 pPosition)
    {
        playerID = id;
        _units = pUnits;
        _position = pPosition;
    }
    public override void ProcessCommand()
    {

    }
}
public class AttackCommand : Command
{
    private List<int> _units;
    public AttackCommand(List<int> pUnits)
    {

    }
    public override void ProcessCommand()
    {

    }
}