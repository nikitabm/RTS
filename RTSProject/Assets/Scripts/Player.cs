using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private SelectObject _objectSelector;
    public int id;
    public Player()
    {

    }
    void Start()
    {
        _objectSelector = gameObject.AddComponent<SelectObject>();
        _objectSelector.SetEnabled(true);
        _objectSelector.PlayerID = 1;
    }

    void Update()
    {
    }
}
