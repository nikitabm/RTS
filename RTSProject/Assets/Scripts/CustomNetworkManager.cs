using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomNetworkManager : NetworkManager
{

    void Start()
    {
        SetPort();
        NetworkManager.singleton.StartHost();

    }
    public void JoinGame()
    {
        SetPort();
        NetworkManager.singleton.StartClient();

    }
    void SetIPAddress()
    {
		string ipAddress=GameObject.Find("InputFieldIPAddress").transform.Find("Text").GetComponent<Text>().text;
		NetworkManager.singleton.networkAddress=ipAddress;
    }

    void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }

}
