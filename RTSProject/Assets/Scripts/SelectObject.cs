using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SelectObject : MonoBehaviour
{
    private Vector3 _clickPoint = Vector3.zero;
    private List<Unit> _units = new List<Unit>();
    private Vector3 _mousePosition;
    private bool _dragging;
    [SerializeField]
    private bool _enabled;
    private int _playerID;
    public delegate void OnCommandCreated(Command m);
    public static event OnCommandCreated commandCreated;
    public delegate void UnitSelected();
    public static UnitSelected onSelected;
    public delegate void UnitDeselected();
    public static UnitDeselected onDeselected;
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
        UnitSelected,
        SelectedLocation,
        MoveCommandComplete
    }

    public StateOfPlayer playerState;
    private void Start()
    {
        playerState = StateOfPlayer.Idle;
        ServiceLocator.GetService<CommandManager>().SubsribeToEvent();
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
            ScreenHelper.DrawScreenRect(rect, Color.green);
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
                if (playerState == StateOfPlayer.Idle)
                {
                    GameObject obj = hit.transform.gameObject;
                    if (obj.GetComponent(typeof(ISelectable)) != null)
                    {
                        _units.Clear();
                        var unit = obj.transform.gameObject.GetComponent<Unit>();
                        _units.Add(unit);
                        playerState = StateOfPlayer.UnitSelected;
                        onSelected();

                    }
                    else
                    {
                        _dragging = true;
                    }
                }
                else if (playerState == StateOfPlayer.UnitSelected)
                {
                    _units.Clear();
                    playerState = StateOfPlayer.Idle;
                    //_clickPoint = hit.point;
                    //playerState = StateOfPlayer.SelectedLocation;
                    //CreateAndPassCommand(_units[0].ID, _clickPoint);
                    //playerState = StateOfPlayer.Idle;

                }
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (playerState == StateOfPlayer.UnitSelected)
                {
                    _clickPoint = hit.point;
                    playerState = StateOfPlayer.SelectedLocation;
                    CreateAndPassCommand(_units[0].ID, _clickPoint);
                    playerState = StateOfPlayer.Idle;
                }
            }
        }
    }
    public void CreateAndPassCommand(int i, Vector3 pos)
    {
        //TODO: change to ARRAY
        List<int> temp = new List<int>();
        temp.Add(i);
        Command issuedCommand;
        issuedCommand = Command.CreateCommand<MoveCommand>(temp, pos);
        commandCreated(issuedCommand);
        if (ServiceLocator.GetService<GameManager>().movementWithoutNetwork)
        {
            ServiceLocator.GetService<CommandManager>().PassCommandsToUnits();
        }
    }
}
