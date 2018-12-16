using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, ISelectable
{


    public int ID = 0;
    private GameManager _gm;
    void Start()
    {
        _gm = ServiceLocator.GetService<GameManager>();
        _gm.AddToTeam(0, this.gameObject);
        // gameObject.transform.SetPositionAndRotation(
        //      new Vector3(Mathf.Round(gameObject.transform.position.x),
        //      gameObject.transform.position.y,
        //       Mathf.Round(gameObject.transform.position.z)), Quaternion.identity);
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
