using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, Service
{
    public GameObject TeamOneController;
    public GameObject TeamTwoController;

    //unused
    private List<GameObject> _teamOneUnits = new List<GameObject>();
    private List<GameObject> _teamTwoUnits = new List<GameObject>();
    private Unit[] _units;
    private Player _player;

    private void Awake()
    {
        InitializeServices();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {

        }
    }
    public void AddUnit(int id, Unit pUnit)
    {
        _units[id] = pUnit;
    }
    public Unit GetUnit(int id)
    {
        return _units[id];
    }
    public Player GetPlayer()
    {
        return _player;
    }
    public Player CreatePlayer()
    {
        if (_player == null)
            _player = gameObject.AddComponent<Player>();

        return _player;

    }
    private void InitializeServices()
    {
        ServiceLocator.ProvideService(this);
    }
    public void AddToTeam(int n, GameObject g)
    {
        if (n == 0) _teamOneUnits.Add(g);
        else _teamTwoUnits.Add(g);

    }
    // public GameObject getUnit(int index)
    // {
    //     for (int i = 0; i < _teamOneUnits.Count; i++)
    //     {
    //         if (_teamOneUnits[i].GetComponent<Unit>().ID == index) return _teamOneUnits[i];

    //     }
    //     return null;
    // }
    public List<GameObject> GetTeamOneUnits()
    {
        return _teamOneUnits;
    }

    public List<GameObject> GetTeamTwoUnits()
    {
        return _teamTwoUnits;
    }

}
