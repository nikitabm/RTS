using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, Service
{
    public GameObject TeamOneController;
    public GameObject TeamTwoController;
    public bool movementWithoutNetwork;

    //unused
    private List<GameObject> _teamOneUnits = new List<GameObject>();
    private List<GameObject> _teamTwoUnits = new List<GameObject>();
    private Dictionary<int, UnitScript> _units = new Dictionary<int, UnitScript>();
    public Dictionary<int, UnitScript> Units
    {
        get { return _units; }
    }
    private Player _player;

    private void Awake()
    {
        InitializeServices();
    }
    private void Update()
    {
        if (movementWithoutNetwork && _player == null)
        {
            CreatePlayer();
        }
    }

    public void AddUnit(int id, UnitScript pUnit)
    {
        _units[id] = pUnit;
    }
    public UnitScript GetUnit(int id)
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

    public List<GameObject> GetTeamOneUnits()
    {
        return _teamOneUnits;
    }

    public List<GameObject> GetTeamTwoUnits()
    {
        return _teamTwoUnits;
    }

}
