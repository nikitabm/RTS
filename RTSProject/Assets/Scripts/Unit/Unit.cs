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
    }

    private void Update()
    {


        if (_currentCommand.GetType() != typeof(EmptyCommand))
        {
            MoveToLocation(_currentCommand.position);
            //Flocking(_currentCommand.units);
        }

    }
    public void Select()
    {
        GetComponent<Renderer>().material = SelectMaterial;
    }
    public void Deselect()
    {
        GetComponent<Renderer>().material = DeselectMaterial;
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
        var desiredVelocity = Vector3.Normalize(target - transform.position + _offset) * _maxVelocity;
        var steering = desiredVelocity;
        steering /= _mass;
        _velocity = steering;
        if (Vector3.Distance(transform.position, target + _offset) < 0.1)
            _currentCommand = new EmptyCommand();
        if (_velocity != null)
            transform.position = transform.position + _velocity;
    }
    private void Flocking(List<int> units)
    {
        var steer = Vector3.zero;
        foreach (int unitID in units)
        {
            Unit u = _gm.GetUnit(unitID);
            var distance = Vector3.Distance(transform.position, u.transform.position);
            if (distance < _desiredSeparation)
            {
                var difference = transform.position - u.transform.position;
                difference = Vector3.Normalize(difference);
                difference = _scalar * difference;
                difference = difference / distance;
                steer += difference;
            }
        }
        if (units.Count > 0)
        {
            steer = steer / units.Count;
        }
        _velocity += steer;
    }
}
