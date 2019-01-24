using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

public abstract class Command
{

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

    public virtual void Execute()
    {
    }
}