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
    private void Start()
    {


    }

    private void Update()
    {

    }

    public void ToggleLogs()
    {
        Log.SetActive(!Log.activeSelf);
    }
    public void CreateBuilding()
    {
        var buildingPlacement=GameObject.FindGameObjectWithTag("Overseer").GetComponent<BuildingPlacement>();
        for (int i = 0; i < buildingPlacement.buildings.Count; i++)
        {
            if (gameObject.name == buildingPlacement.buildings[i].name)
            {
                buildingPlacement.currentBuilding = Instantiate(buildingPlacement.buildings[i]);
            }
        }
    }

    public void ToggleNetworkMenu()
    {
        NetworkMenu.SetActive(!NetworkMenu.activeSelf);
    }

    public static void WriteDataToFile(string s)
    {
        if (File.Exists(fileName))
        {
            StreamWriter writer = new StreamWriter(fileName, true);
            writer.WriteLine(s);
            writer.Close();
            return;
        }
        else
        {
            print("Created Log file");
            var sr = File.CreateText(fileName);
            sr.WriteLine(s);
            sr.Close();
        }
    }
}
