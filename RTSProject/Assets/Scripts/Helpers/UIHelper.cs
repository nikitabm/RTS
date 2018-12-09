using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelper : MonoBehaviour
{
    [SerializeField]
    private GameObject Log;
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
}
