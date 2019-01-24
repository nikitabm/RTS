using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, ISelectable
{
    [SerializeField]
    private Command _currentCommand = null;
    private Queue<Command> _commandQueue = new Queue<Command>();
    private bool _selected;
    private Vector3 _targetPos;
    private GameManager _gm;
    private Vector3 movePoint;

    [SerializeField]
    private List<Unit> _units = new List<Unit>();

    [SerializeField]
    private Vector3 _offset;

    public Material SelectMaterial;
    public Material DeselectMaterial;
    public int ID;
    private Mobile _unitMovement;


    public Command CurrentCommand
    {
        get
        { return _currentCommand; }
        set
        { _currentCommand = value; }
    }

    public bool Selected
    {
        get { return _selected; }
        set { _selected = value; }
    }

    private void Awake()
    {
        _currentCommand = new EmptyCommand();
    }

    private void Start()
    {
        _unitMovement = GetComponent<Mobile>();
        _gm = ServiceLocator.GetService<GameManager>();
        _gm.AddUnit(ID, this);
        CommandManager.OnCommandExecute += ExecuteCurrentCommand;
    }


    public void Select()
    {
        GetComponent<Renderer>().material = SelectMaterial;
    }

    public void Deselect()
    {
        GetComponent<Renderer>().material = DeselectMaterial;
    }

    public void SetCommand(Command pCommand)
    {

        float posX = ((pCommand as MoveCommand).units.IndexOf(this.ID) % _gm.rowLength) * _gm.formationSeparation;
        float posZ = ((pCommand as MoveCommand).units.IndexOf(this.ID) / _gm.rowLength) * _gm.formationSeparation;

        Vector3 pos = (pCommand as MoveCommand).position - new Vector3(posX, 0, posZ) +
            new Vector3(((_gm.rowLength * _gm.formationSeparation) / 2), 0,
            (((pCommand as MoveCommand).units.Count / _gm.rowLength) * _gm.formationSeparation) / 2);

        //_currentCommand.position = pos;
        //_currentCommand.units = pCommand.units;

        _unitMovement.SetWalkabilityOfCurrentNode(true);
        _unitMovement.RequestPath(pos);

    }

    public void ExecuteCurrentCommand()
    {
        //?
    }
}
