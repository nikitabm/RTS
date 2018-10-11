using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SelectObject))]
public class PlayerController : MonoBehaviour
{

    private SelectObject _selectObject;

    void Start()
    {
        _selectObject = GetComponent<SelectObject>();

    }

    void Update()
    {
        _selectObject.ClickOnObjects();
    }
}
