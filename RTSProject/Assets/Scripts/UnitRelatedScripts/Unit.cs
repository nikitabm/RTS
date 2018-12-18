using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, ISelectable
{
    [SerializeField]
    private Command _currentCommand;
    private Queue<Command> _commandQueue = new Queue<Command>();
    private IEnumerator coroutine;
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
        _gm = ServiceLocator.GetService<GameManager>();
        _gm.AddUnit(ID, this);
        RoundPos();
        CommandManager.OnCommandExecute += ExecuteCurrentCommand;

    }
    public void RoundPos()
    {
        gameObject.transform.position = new Vector3(
           Mathf.Round(gameObject.transform.position.x),
           gameObject.transform.position.y,
          Mathf.Round(gameObject.transform.position.z));
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ExecuteCurrentCommand();
        }
    }
    public void ExecuteCurrentCommand()
    {
        if (_currentCommand != null)
        {
            StartCoroutine(AllignOnX(new Vector3(
                Mathf.Round(_currentCommand.position.x),
                0.5f,
                Mathf.Round(_currentCommand.position.z))));
        }
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
            yield return new WaitForSeconds(1.0f);
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
            yield return new WaitForSeconds(1.0f);
            gameObject.transform.position += new Vector3(0, 0, step);
        }
    }
}
