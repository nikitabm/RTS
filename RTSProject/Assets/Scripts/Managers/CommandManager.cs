using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{


    private Queue<Command> commandQueue = new Queue<Command>();
    void Start()
    {
        //responsible for queuing commands and operating with them
    }

    void Update()
    {

    }
}
