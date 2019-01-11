using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelper : MonoBehaviour
{
    [SerializeField]
    private GameObject Log;
    [SerializeField]
    private GameObject NetworkMenu;
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
}
