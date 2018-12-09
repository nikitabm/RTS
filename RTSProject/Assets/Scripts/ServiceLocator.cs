using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ServiceLocator : MonoBehaviour
{
    private static List<Service> _services = new List<Service>();

    public static Service GetService(Type type)
    {

        return _services.Find(s => s.GetType() == type);
    }
    public static T GetService<T>() where T : Service
    {
        return (T)GetService(typeof(T));
    }
    // public static T GetService<T>() where T : Service
    // {
    //     return GetService(typeof(T));
    // }

    public static void RemoveService(Type type)
    {
        _services[_services.IndexOf(GetService(type))] = null;
    }

    public static void ProvideService(Service service)
    {
        _services.Add(service);
    }

    public static void RemoveService<T>()
    {
        RemoveService(typeof(T));
    }
}

