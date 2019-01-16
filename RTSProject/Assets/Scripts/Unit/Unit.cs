using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, ISelectable
{
    [SerializeField]
    private Command _currentCommand = null;
    private Queue<Command> _commandQueue = new Queue<Command>();
    private IEnumerator coroutine;
    private bool _selected;
    private Vector3 _targetPos;
    private NavMeshAgent _agent;
    private GameManager _gm;
    private Vector3 movePoint;
    private Rigidbody _rb;
    [SerializeField]
    private List<Unit> _units = new List<Unit>();
    [SerializeField]
    private float _maxVelocity;
    [SerializeField]
    private float _desiredSeparation;
    [SerializeField]
    private float _scalar;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _acceleration;
    [SerializeField]
    private Vector3 _offset;
    [SerializeField]
    private float _mass;

    public Material SelectMaterial;
    public Material DeselectMaterial;
    private List<GameObject> points = new List<GameObject>();
    public int ID = 0;


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
        _rb = gameObject.GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _gm = ServiceLocator.GetService<GameManager>();
        _gm.AddUnit(ID, this);
        CommandManager.OnCommandExecute += ExecuteCurrentCommand;
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("team1Unit"))
        {
            _units.Add(o.GetComponent<Unit>());
        }
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("point"))
        {
            points.Add(o);
        }
        _currentCommand.position = points[0].transform.position;
    }

    private void Update()
    {
        MoveToLocation(_currentCommand.position);
        Flocking(_currentCommand.units);
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
        _currentCommand.position = pCommand.position;
        _currentCommand.units = pCommand.units;
    }
    public void RoundPos()
    {
        gameObject.transform.position = new Vector3(
           Mathf.Round(gameObject.transform.position.x),
           gameObject.transform.position.y,
          Mathf.Round(gameObject.transform.position.z));
    }

    public void ExecuteCurrentCommand()
    {
        //?
    }

    private IEnumerator AllignOnX(Vector3 movePoint)
    {
        int step;
        int pX = (int)movePoint.x;
        int numberOfSteps = (int)transform.position.x - pX;
        if (numberOfSteps > 0) step = -1;
        else step = 1;

        for (int i = 0; i < Mathf.Abs(numberOfSteps); i++)
        {
            yield return new WaitForSeconds(1);
            gameObject.transform.position += new Vector3(step, 0, 0);
        }
        yield return StartCoroutine(AllignOnZ((int)movePoint.z));
    }

    private IEnumerator AllignOnZ(int pZ)
    {

        int step;
        int numberOfSteps = (int)transform.position.z - pZ;
        if (numberOfSteps > 0) step = -1;
        else step = 1;

        for (int i = 0; i < Mathf.Abs(numberOfSteps); i++)
        {
            yield return new WaitForSeconds(1);
            gameObject.transform.position += new Vector3(0, 0, step);
        }
    }

    private void MoveToLocation(Vector3 target)
    {
        if (target == Vector3.zero) return;
        var desiredVelocity = Vector3.Normalize(target + _offset - transform.position) * _maxVelocity;
        var steering = desiredVelocity;
        steering /= (_mass > 0) ? _mass : 1;
        _velocity = steering;
        transform.position = transform.position + _velocity;

        if (Vector3.Distance(target + _offset, transform.position) < 0.1f)
        {
            //_currentCommand = new EmptyCommand();
            points.RemoveAt(0);
            if (points.Count == 0)
            {
                _currentCommand.position = Vector3.zero;

            }
            else
            {
                _currentCommand.position = points[0].transform.position;
            }
        }
    }

    private void Flocking(List<int> units)
    {
        var steer = Vector3.zero;
        if (units != null)
        {
            foreach (int unitID in units)
            {
                Unit u = _gm.GetUnit(unitID);
                if (u != null)
                {
                    var distance = Vector3.Distance(transform.position, u.transform.position);
                    if (distance < _desiredSeparation)
                    {
                        var difference = transform.position - u.transform.position;
                        difference = Vector3.Normalize(difference);
                        if (distance > 0)
                            steer += difference / distance;
                    }
                }
            }
            if (units.Count > 0)
            {
                steer = steer / units.Count;
            }
            transform.position = transform.position + steer;
        }
    }
}
