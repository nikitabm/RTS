using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SelectObject : MonoBehaviour
{
    [SerializeField]
    private List<UnitScript> _units = new List<UnitScript>();

    private Vector3 _clickPoint = Vector3.zero;
    private Vector3 _mousePos;
    private bool _dragging;
    private bool _enabled;
    private GameManager _gm;
    private int _colLength;
    private int _col;
    private int _row;
    private int _seperation;

    public delegate void OnCommandCreated(Command m);
    public static event OnCommandCreated СommandCreated;

    public enum StateOfPlayer
    {
        Idle,
        SelectBuilding,
        UnitsSelected,
        SelectedLocation,
        MoveCommandComplete
    }

    public StateOfPlayer playerState;
    private void Start()
    {
        playerState = StateOfPlayer.Idle;
        ServiceLocator.GetService<CommandManager>().SubsribeToEvent();
        _gm = ServiceLocator.GetService<GameManager>();
    }
    private void Update()
    {
        ClickOnObjects();
    }
    private void OnGUI()
    {
        if (_dragging)
        {
            var rect = ScreenHelper.GetScreenRect(_mousePos, Input.mousePosition);
            ScreenHelper.DrawScreenRectBorder(rect, 1.0f, Color.green);
        }
    }
    public void SetEnabled(bool value)
    {
        _enabled = value;
    }
    public Vector3 GetClickPoint()
    {
        return _clickPoint;
    }
    public void GetLocationInFormation(List<GameObject> units)
    {
        var selectedLoc = Vector3.zero;
        foreach (GameObject o in units)
        {
            var pos = selectedLoc + new Vector3(_col * _seperation, 0, _row * _seperation);
            _col += 1;
            if (_col == _colLength)
            {
                _col = 0;
                _row += 1;
            }
        }

    }
    public void ClickOnObjects()
    {
        if (!_enabled) return;
        if (Input.GetMouseButtonDown(0))
        {
            _mousePos = Input.mousePosition;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                GameObject obj = hit.transform.gameObject;
                if (obj.GetComponent(typeof(ISelectable)) != null)
                {
                    foreach (UnitScript u in _units)
                    {
                        u.Deselect();
                    }
                    _units.Clear();
                    var unit = obj.transform.gameObject.GetComponent<UnitScript>();
                    _units.Add(unit);

                    foreach (UnitScript u in _units)
                    {
                        u.Select();
                    }
                    playerState = StateOfPlayer.UnitsSelected;
                }
                else
                {
                    _units.Clear();
                    playerState = StateOfPlayer.Idle;
                    _dragging = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            foreach (var unit in _gm.Units)
            {
                if (IsInSelection(unit.Value.transform))
                {
                    _units.Add(unit.Value);
                    unit.Value.Select();
                }
                if (!_units.Contains(unit.Value))
                    unit.Value.Deselect();
            }
            if (_units.Count > 0)
            {
                playerState = StateOfPlayer.UnitsSelected;
            }
            else
            {
                playerState = StateOfPlayer.Idle;
            }
            _dragging = false;
        }

        if (Input.GetMouseButtonUp(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (playerState == StateOfPlayer.UnitsSelected)
                {
                    _clickPoint = hit.point;
                    CreateAndPassCommand(_units, _clickPoint);
                    //playerState = StateOfPlayer.Idle;
                }
            }
        }
    }

    private bool IsInSelection(Transform t)
    {
        if (!_dragging) return false;
        var camera = Camera.main;
        var viewportBounds = ScreenHelper.GetViewportBounds(camera, _mousePos, Input.mousePosition);
        return viewportBounds.Contains(camera.WorldToViewportPoint(t.position));
    }

    public void CreateAndPassCommand(List<UnitScript> pUnits, Vector3 pos)
    {
        List<int> temp = new List<int>();
        foreach (var unit in pUnits)
        {
            temp.Add(unit.ID);
        }

        Command issuedCommand;
        issuedCommand = Command.CreateCommand<MoveCommand>(temp, pos);
        СommandCreated(issuedCommand);
        if (ServiceLocator.GetService<GameManager>().movementWithoutNetwork)
        {
            ServiceLocator.GetService<CommandManager>().PassCommandsToUnits();
            ServiceLocator.GetService<CommandManager>()._allCommands.Clear();
        }
    }
}
