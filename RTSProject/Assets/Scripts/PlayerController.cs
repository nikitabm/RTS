using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(SelectObject))]
public class PlayerController : MonoBehaviour
{

 

    public enum StateOfPlayer
    {
        Idle,
        SelectBuilding,
        SelectUnit,
        WaitingToSelectLocation,
        SelectedLocation
    }
    public List<MoveCommand> _commandList = null;
    public SelectObject ObjectSelector;
    public GameObject _selectedObj;
    public Vector3 _clickPosition;
    public StateOfPlayer _playerState;
    private void Start()
    {
       
        ObjectSelector = GetComponent<SelectObject>();
        _playerState = StateOfPlayer.Idle;
        _clickPosition = Vector3.zero;
    }

    private void Update()
    {
        ObjectSelector.ClickOnObjects();
        _selectedObj = ObjectSelector.GetSelectedObject();
        _clickPosition = ObjectSelector.GetClickPoint();
        // RegisterCommands();
    }

    
}
