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
                        playerState = StateOfPlayer.SelectUnit;
                        units.Add(obj.transform.gameObject.GetComponent<Unit>().ID);
                    }
                }
                else if (playerState == StateOfPlayer.SelectUnit)
                {
                    _clickPoint = hit.point;
                    playerState = StateOfPlayer.SelectedLocation;
                    CreateAndPassCommand(units, _clickPoint);
                    playerState = StateOfPlayer.Idle;

                }
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            playerState = StateOfPlayer.Idle;
        }
    }
    public void CreateAndPassCommand(List<int> pUnits, Vector3 pos)
    {
        //create
        CustomMoveCommand inputCommand = new CustomMoveCommand(pUnits, pos);
        //send
        (ServiceLocator.GetService(typeof(LockStepManager)) as LockStepManager).SetInputCommand(inputCommand);
    }
}
