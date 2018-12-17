using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, ISelectable
{

    private Command _currentCommand;
    private Queue<Command> _commandQueue = new Queue<Command>();
    public Command CurrentCommand
    {
        get
        { return _currentCommand; }
        set
        { _currentCommand = value; }
    }
    public int ID = 0;
    private GameManager _gm;


    void Start()
    {
        _gm = ServiceLocator.GetService<GameManager>();
        _gm.AddToTeam(0, this.gameObject);
        RoundPos();

    }
    public void RoundPos()
    {
        gameObject.transform.position = new Vector3(
           Mathf.Round(gameObject.transform.position.x),
           gameObject.transform.position.y,
          Mathf.Round(gameObject.transform.position.z));
    }
    

    void Update()
    {

    }
}
