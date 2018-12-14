using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

[System.Serializable]
public abstract class Command
{
    public int playerID;
    public Vector3 _position;

    public List<int> _units;
    public static T CreateCommand<T>(List<int> pUnits, Vector3 pPos) where T : Command
    {
        return (T)Activator.CreateInstance(typeof(T), pUnits, pPos);
    }
    public static T CreateCommand<T>(Vector3 pPos, int pAction) where T : Command
    {
        return (T)Activator.CreateInstance(typeof(T), pPos, pAction);
    }
}