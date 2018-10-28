using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour
{

    private GameObject _selectedObject = null;
    private Vector3 _clickPoint=Vector3.zero;
    private Color _objectColor;
    private Color _selectionColor;
    public PlayerController.StateOfPlayer playerState;
    void Start()
    {
        _selectionColor=Color.blue;
        playerState=PlayerController.StateOfPlayer.Idle;
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
        if (Input.GetMouseButtonUp(0))
        {   
            
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                GameObject obj=hit.transform.gameObject;
                if(obj.GetComponent(typeof(ISelectable))!=null)
                {
                    //debug
                    playerState = PlayerController.StateOfPlayer.SelectUnit;
                    _selectedObject=obj;
                    playerState = PlayerController.StateOfPlayer.WaitingToSelectLocation;
                }
               else
               {
                    _clickPoint= hit.point;
                    playerState = PlayerController.StateOfPlayer.SelectedLocation;
                }
            }
        }
        if(Input.GetMouseButtonUp(2))
        {
            playerState = PlayerController.StateOfPlayer.Idle;
            _selectedObject=null;
        }
    }
}
