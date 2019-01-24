using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private List<Unit> _units = new List<Unit>();

    private Vector3 _clickPoint = Vector3.zero;
    private Vector3 _mousePos;
    private bool _dragging;
    private bool _enabled;
    private GameManager _gm;
    private int _rowLength = 5;
    private int _col;
    private int _row;
    private int _seperation = 5;

    public delegate void OnCommandCreated(Command m);
    public static event OnCommandCreated CommandCreated;

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
        if (Input.GetButtonDown("Escape"))
        {
            CreateAndPassCommand(new PauseCommand());
        }
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

    public void CalculateLocationInFormation(Vector3 point, List<GameObject> units)
    {
        var selectedLoc = point - new Vector3((_rowLength / 2) * _seperation, 0, ((units.Count / _rowLength) * _seperation) / 2);
        for (int i = 0; i < units.Count; i++)
        {
            var pos = selectedLoc + new Vector3(_col * _seperation, 0, _row * _seperation);
            _col += 1;
            if (_col == _rowLength)
            {
                _col = 0;
                _row += 1;
            }
        }
        _col = 0;
        _row = 0;
    }

    public void CalculateLocationInFormation(Vector3 point, List<Unit> units)
    {
        var selectedLoc = point - new Vector3((_rowLength / 2) * _seperation, 0, ((units.Count / _rowLength) * _seperation) / 2);
        for (int i = 0; i < units.Count; i++)
        {
            var pos = selectedLoc + new Vector3(_col * _seperation, 0, _row * _seperation);
            _col += 1;
            if (_col == _rowLength)
            {
                _col = 0;
                _row += 1;
            }
            var unit = units[i].gameObject.GetComponent<UnitMovement>();
            unit.SetWalkabilityOfCurrentNode(true);
            unit.RequestPath(pos);
        }
        _col = 0;
        _row = 0;
    }

    public void CalculateLocationInFormation(Vector3 point, int unitNumber)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.position = point;
        var selectedLoc = point - new Vector3((_rowLength / 2) * _seperation, 0, ((unitNumber / _rowLength) * _seperation) / 2);
        for (int i = 0; i < unitNumber; i++)
        {
            var pos = selectedLoc - new Vector3(_col * _seperation, 0, _row * _seperation);
            _col += 1;
            if (_col == _rowLength)
            {
                _col = 0;
                _row += 1;
            }

        }
        _col = 0;
        _row = 0;

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
                    foreach (Unit u in _units)
                    {
                        u.Deselect();
                    }
                    _units.Clear();
                    var unit = obj.transform.gameObject.GetComponent<Unit>();
                    _units.Add(unit);

                    foreach (Unit u in _units)
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
                    //CalculateLocationInFormation(_clickPoint, _units);
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

    public void CreateAndPassCommand(List<Unit> pUnits, Vector3 pos)
    {
        List<int> temp = new List<int>();
        foreach (var unit in pUnits)
        {
            temp.Add(unit.ID);
        }

        Command issuedCommand;
        issuedCommand = Command.CreateCommand<MoveCommand>(temp, pos);
        CommandCreated(issuedCommand);
        if (ServiceLocator.GetService<GameManager>().movementWithoutNetwork)
        {
            ServiceLocator.GetService<CommandManager>().PassCommandsToUnits();
            ServiceLocator.GetService<CommandManager>()._allCommands.Clear();
        }
    }
    public void CreateAndPassCommand(Command c)
    {
        CommandCreated(c);
    }
}
