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
    private List<MoveCommand> _commandList = null;
    private SelectObject ObjectSelector;
    private GameObject _selectedObj;
    private Vector3 _clickPosition;
    private StateOfPlayer _playerState;
    private void Start()
    {
        _commandList = new List<MoveCommand>();
        ObjectSelector = GetComponent<SelectObject>();
        _playerState = StateOfPlayer.Idle;
        _clickPosition = Vector3.zero;
    }

    private void Update()
    {
        ObjectSelector.ClickOnObjects();
        _selectedObj = ObjectSelector.GetSelectedObject();
        _clickPosition = ObjectSelector.GetClickPoint();
        RegisterCommands();
    }

    private void RegisterCommands()
    {
        if (_selectedObj != null && ObjectSelector.playerState == StateOfPlayer.SelectedLocation)
        {
            MoveCommand moveCommand=new MoveCommand();
            moveCommand._unit=_selectedObj;
            moveCommand._position=_clickPosition;
            _clickPosition = Vector3.zero;
            _selectedObj = null;
            ObjectSelector.playerState = StateOfPlayer.Idle;
            _commandList.Add(moveCommand);
            print(moveCommand._unit);
            print(moveCommand._position);
        }
    }
}
