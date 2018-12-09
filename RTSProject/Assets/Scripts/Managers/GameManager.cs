using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, Service
{
    //public
    public GameObject TeamOneController;
    public GameObject TeamTwoController;


    //private
    private List<GameObject> _teamOneUnits = new List<GameObject>();
    private List<GameObject> _teamTwoUnits = new List<GameObject>();
    private Player _player;

    private void Awake()
    {
        InitializeServices();

    }
    public void Start()
    {
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
