using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

[System.Serializable]
public abstract class Command
{
    [SerializeField]

    public Vector3 position;
    [SerializeField]
    public List<int> units = new List<int>{-1};

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

public virtual void Execute()
{ }
}