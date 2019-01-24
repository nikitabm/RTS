using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, Service
{
    public GameObject TeamOneController;
    public GameObject TeamTwoController;
    public bool movementWithoutNetwork;
    public int formationSeparation;
    public int rowLength;
    private bool _paused = false;
    public bool GamePaused
    {
        get { return _paused; }
        set { _paused = value; }
    }


    //unused
    private List<GameObject> _teamOneUnits = new List<GameObject>();
    private List<GameObject> _teamTwoUnits = new List<GameObject>();
    private Dictionary<int, Unit> _units = new Dictionary<int, Unit>();
    public Dictionary<int, Unit> Units
    {
        get { return _units; }
    }
    private Player _player;

    private void Awake()
    {
        //TODO: delete this after testing
        List<GameObject> l = new List<GameObject>();
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("team1Unit"))
        {
            l.Add(o);
        }
        for (int i = 0; i < l.Count; i++)
        {
            l[i].GetComponent<Unit>().ID = i;
        }
        InitializeServices();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (movementWithoutNetwork && _player == null)
        {
            CreatePlayer();
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

    public List<GameObject> GetTeamOneUnits()
    {
        return _teamOneUnits;
    }

    public List<GameObject> GetTeamTwoUnits()
    {
        return _teamTwoUnits;
    }

}
