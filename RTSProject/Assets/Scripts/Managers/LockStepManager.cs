using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Security;
using UnityEngine.UI;


public class LockStepManager : MonoBehaviour, Service
{
    public PlayerCommandsData commandToSend;
    public delegate void OnNextTurn();
    public static event OnNextTurn NextTurn;
    private bool _gameStarted;
    private float _accumilatedTime = 0f;
    private float _frameLength = 0.50f; //FIXME: should be 50 ms


    private void Awake()
    {

    }
    void Start()
    {
        ServiceLocator.ProvideService(this);
        //_frameLength = ServiceLocator.GetService<NetworkingManager>().turnTime;
    }

    public void StartGame()
    {
        _gameStarted = true;
    }
    public void Update()
    {
        if (!_gameStarted) return;

        //Basically same logic as FixedUpdate, but we can scale it by adjusting FrameLength
        _accumilatedTime = _accumilatedTime + Time.deltaTime;
        //in case the FPS is too slow, we may need to update the game multiple times a frame
        while (_accumilatedTime > _frameLength)
        {
            NextTurn();
            print("turn: " + ServiceLocator.GetService<NetworkingManager>().turn);
            _accumilatedTime = _accumilatedTime - _frameLength;
        }
    }
}
