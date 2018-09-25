using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SelectObject))]
public class PlayerController : MonoBehaviour
{
	 public enum ECommandType
    {
        CM_INVALID,
        // CM_ATTACK,
        CM_MOVE
    };
	private SelectObject _selectObject;
	private  ECommandType _command;
    void Start()
    {
		_selectObject=GetComponent<SelectObject>();
	_command= ECommandType.CM_INVALID;

    }

    void Update()
    {
		_selectObject.ClickOnObjects();
    }
}
