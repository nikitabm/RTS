using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour
{

    private GameObject _selectedObject = null;
    private Vector3 _clickPoint = Vector3.zero;
    private List<int> units = new List<int>();
    private Color _objectColor;
    private Color _selectionColor;
    private bool _enabled;
    private int _playerID;
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
        SelectUnit,
        SelectedLocation,
        MoveCommandComplete
    }

    public StateOfPlayer playerState;
    void Start()
    {
        _selectionColor = Color.blue;
        playerState = StateOfPlayer.Idle;
        ServiceLocator.GetService<CommandManager>().SubsribeToEvent();
    }
    void Update()
    {
        ClickOnObjects();
    }
    public void SetEnabled(bool value)
    {
        _enabled = value;
    }
    public GameObject GetSelectedObject()
    {
        return _selectedObject;
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
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (playerState == StateOfPlayer.Idle)
                {
                    GameObject obj = hit.transform.gameObject;
                    if (obj.GetComponent(typeof(ISelectable)) != null)
                    {
                        units.Clear();
                        units.Add(obj.transform.gameObject.GetComponent<Unit>().ID);
                        playerState = StateOfPlayer.SelectUnit;
                    }
                }
                else if (playerState == StateOfPlayer.SelectUnit)
                {
                    _clickPoint = hit.point;
                    playerState = StateOfPlayer.SelectedLocation;
                    CreateAndPassCommand(units[0], _clickPoint);
                    playerState = StateOfPlayer.Idle;

                }
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            units.Clear();
            playerState = StateOfPlayer.Idle;
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
    }
}
