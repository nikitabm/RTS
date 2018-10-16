using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, ISelectable
{


    public int ID = 0;
    private GameManager _gm;
    void Start()
    {
         _gm = ServiceLocator.GetService(typeof(GameManager)) as GameManager;
        _gm.AddToTeam(0,this.gameObject);
    }
    void ISelectable.Diselect()
    {

    }
    void ISelectable.Select()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
