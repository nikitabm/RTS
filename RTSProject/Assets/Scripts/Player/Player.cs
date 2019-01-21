using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private PlayerController _objectSelector;
    public int id;
    public Player()
    {

    }
    void Start()
    {
        _objectSelector = gameObject.AddComponent<PlayerController>();
        _objectSelector.SetEnabled(true);
    }

    void Update()
    {
    }
}
