using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour
{

    private Vector3 _clickPoint = Vector3.zero;
    private List<Unit> _units = new List<Unit>();

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
        SelectUnit,
        SelectedLocation,
        MoveCommandComplete
    }

    public StateOfPlayer playerState;
    void Start()
    {
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
                        _units.Clear();
                        var unit = obj.transform.gameObject.GetComponent<Unit>();
                        _units.Add(unit);
                        playerState = StateOfPlayer.SelectUnit;
                        onSelected();

                    }
                }
                else if (playerState == StateOfPlayer.SelectUnit)
                {
                    _clickPoint = hit.point;
                    playerState = StateOfPlayer.SelectedLocation;
                    //FIXME: shouldnt be zero
                    CreateAndPassCommand(0, _clickPoint);
                    playerState = StateOfPlayer.Idle;

                }
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            _units.Clear();
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
        if (ServiceLocator.GetService<GameManager>().movementWithoutNetwork)
        {
            ServiceLocator.GetService<CommandManager>().PassCommandsToUnits();
            // ServiceLocator.GetService<CommandManager>()._allCommands.Clear();
        }
    }
}
