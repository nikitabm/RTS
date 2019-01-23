﻿using System.Collections;
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

    public virtual void Execute()
    {
    }
}