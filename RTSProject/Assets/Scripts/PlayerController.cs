using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(SelectObject))]
public class PlayerController : MonoBehaviour
{




    public List<MoveCommand> _commandList = null;
    public SelectObject ObjectSelector;
    public GameObject _selectedObj;
    public Vector3 _clickPosition;
    private void Start()
    {

        ObjectSelector = GetComponent<SelectObject>();
        _clickPosition = Vector3.zero;
    }

    private void Update()
    {
        ObjectSelector.ClickOnObjects();
    }


}
