using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

[System.Serializable]
public abstract class Command
{


    //TODO: maybe add data variable to indicate what building to build

    public static T CreateCommand<T>() where T : Command
    {
        return (T)Activator.CreateInstance(typeof(T));
    }
    public static T CreateCommand<T>(List<int> pUnits, Vector3 pPos) where T : Command
    {
        return (T)Activator.CreateInstance(typeof(T), pUnits, pPos);
    }

    public static T CreateCommand<T>(int pAction, List<int> pUnits, Vector3 pPos) where T : Command
    {
        return (T)Activator.CreateInstance(typeof(T), pAction, pPos, pAction);
    }


    public static void CommandExecute(Command c)
    {
        Debug.Log("Command execute is called for base class");
    }

    public static void CommandExecute(MoveCommand c)
    {
        c.AssignCommand();
        //Debug.Log(c.ToString());
    }

    public static void CommandExecute(BuildCommand c)
    {
        Debug.Log(c.ToString());
    }

    public static void CommandExecute(AttackCommand c)
    {
        Debug.Log(c.ToString());

    }

    public static void CommandExecute(EmptyCommand c)
    {
        Debug.Log(c.ToString());
    }

    public virtual void Execute()
    {
    }
}