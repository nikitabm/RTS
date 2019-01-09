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
    private NavMeshAgent _agent;
    public Command CurrentCommand
    {
        get
        { return _currentCommand; }
        set
        { _currentCommand = value; }
    }
    public int ID = 0;
    private GameManager _gm;
    Vector3 movePoint;


    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _gm = ServiceLocator.GetService<GameManager>();
        _gm.AddUnit(ID, this);
        CommandManager.OnCommandExecute += ExecuteCurrentCommand;

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
        if (_currentCommand.position != Vector3.zero)
            _agent.SetDestination(_currentCommand.position);
        //StartCoroutine(AllignOnX(new Vector3(
        //    Mathf.Round(_currentCommand.position.x),
        //    0.5f,
        //    Mathf.Round(_currentCommand.position.z))));
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
}
