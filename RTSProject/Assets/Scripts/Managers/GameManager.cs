using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, Service
{
    //public
    public GameObject TeamOneController;
    public GameObject TeamTwoController;
    private Player _player;
    private int _playerID;

    //private
    public List<GameObject> _teamOneUnits = new List<GameObject>();
    public List<GameObject> _teamTwoUnits = new List<GameObject>();

    private void Awake()
    {
        InitializeServices();

    }

    public void CreatePlayer()
    {
        _player = new Player();
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
