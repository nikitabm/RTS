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
    public ECommandType command = ECommandType.CM_INVALID;
    public int PlayerID = 0;
    public int NetworkID = 0;
    public virtual void ProcessCommand()
    {

    }

}
public class MoveCommand : Command
{
    private Vector3 _position;
    private List<int> _units;
    public MoveCommand(List<int> pUnits, Vector3 pPosition)
    {
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
    //todo
    public AttackCommand(List<int> pUnits)
    {

    }
    public override void ProcessCommand()
    {

    }
}