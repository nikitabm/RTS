using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SelectObject : MonoBehaviour
{
    private Vector3 _clickPoint = Vector3.zero;
    [SerializeField]
    private List<Unit> _units = new List<Unit>();
    private Vector3 _mousePosition;
    private bool _dragging;
    [SerializeField]
    private bool _enabled;
    private int _playerID;
    private GameManager _gm;
    public delegate void OnCommandCreated(Command m);
    public static event OnCommandCreated commandCreated;
    public int PlayerID
    {
        get
        {
            return _playerID;
        }
        set
        {
            _playerID = value;
        }
    }
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
            var rect = ScreenHelper.GetScreenRect(_mousePosition, Input.mousePosition);
            //ScreenHelper.DrawScreenRect(rect, Color.green);
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
    public void ClickOnObjects()
    {
        if (!_enabled) return;
        if (Input.GetMouseButtonDown(0))
        {
            _mousePosition = Input.mousePosition;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f))
            {

                GameObject obj = hit.transform.gameObject;
                if (obj.GetComponent(typeof(ISelectable)) != null)
                {
                    _units.Clear();
                    var unit = obj.transform.gameObject.GetComponent<Unit>();
                    _units.Add(unit);
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
                }
            }
            if (_units.Count > 0)
                playerState = StateOfPlayer.UnitsSelected;
            else
                playerState = StateOfPlayer.Idle;
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
                    playerState = StateOfPlayer.SelectedLocation;
                    CreateAndPassCommand(_units, _clickPoint);
                    playerState = StateOfPlayer.Idle;
                }
            }
        }
    }
    private bool IsInSelection(Transform t)
    {
        if (!_dragging) return false;
        var camera = Camera.main;
        var viewportBounds = ScreenHelper.GetViewportBounds(camera, _mousePosition, Input.mousePosition);
        return viewportBounds.Contains(camera.WorldToViewportPoint(t.position));
    }
    public void CreateAndPassCommand(List<Unit> pUnits, Vector3 pos)
    {
        //TODO: change to ARRAY
        List<int> temp = new List<int>();
        foreach (var unit in pUnits)
        {
            temp.Add(unit.ID);
            print(unit.ID);
        }
        Command issuedCommand;
        issuedCommand = Command.CreateCommand<MoveCommand>(temp, pos);
        commandCreated(issuedCommand);
        if (ServiceLocator.GetService<GameManager>().movementWithoutNetwork)
        {
            ServiceLocator.GetService<CommandManager>().PassCommandsToUnits();
        }
    }
}
