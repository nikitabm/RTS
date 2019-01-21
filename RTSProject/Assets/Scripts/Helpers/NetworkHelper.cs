using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading;

public class NetworkHelper
{
    public enum ServerCommand
    {
        StartGame,
        PauseGame,
        UnpauseGame,
        NextTurn,
        SendChatMessage
    }
    public enum ClientCommand
    {
        PauseGame,
        UnpauseGame,
        BuildBuilding,
        HireUnits,
        MoveUnits
    }
    public NetworkHelper()
    {

    }
}
