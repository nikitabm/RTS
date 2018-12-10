using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Command
{
    public int playerID;
    public int NetworkID;
    public Vector3 _position;

    public List<int> _units;
    public Command(int id, List<int> pUnits, Vector3 pPosition)
    {
        playerID = id;
        //TODO: change
        NetworkID = 0;
        _units = pUnits;
        _position = pPosition;
    }
    // public static Command CreateCommand(List<int> pUnits, Vector3 pos)
    // {

    //     // return new MoveCommand();
    // }
}
// public class EmptyCommand : Command
// {

// }

// [System.Serializable]
// public class MoveCommand : Command
// {
//     // int playerID;

//     public MoveCommand(int id, List<int> pUnits, Vector3 pPosition)
//     {
//         playerID = id;
//         _units = pUnits;
//         _position = pPosition;
//     }
//     public override void ProcessCommand()
//     {

//     }
// }
// public class AttackCommand : Command
// {
//     private List<int> _units;
//     public AttackCommand(List<int> pUnits)
//     {

//     }
//     public override void ProcessCommand()
//     {

//     }
// }