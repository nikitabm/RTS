using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class UIHelper : MonoBehaviour
{
    [SerializeField]
    private GameObject Log;
    [SerializeField]
    private GameObject NetworkMenu;
    private static string fileName = "log.txt";
    void Start()
    {

    }

    void Update()
    {

    }
    public void ToggleLogs()
    {
        Log.SetActive(!Log.activeSelf);
    }
    public void ToggleNetworkMenu()
    {
        NetworkMenu.SetActive(!NetworkMenu.activeSelf);
    }
    public static void WriteDataToFile(string s)
    {
        if (File.Exists(fileName))
        {
            Debug.Log(fileName + " already exists.");
            return;
        }
        var sr = File.CreateText(fileName);
        sr.WriteLine(s);
        sr.Close();
    }
}
