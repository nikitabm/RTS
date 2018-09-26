using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SelectObject : MonoBehaviour
{

    private GameObject _selectedObject = null;
    private Color _objectColor;
    private Color _selectionColor;
    void Start()
    {
        _selectionColor=Color.blue;
    }

    public void ClickOnObjects()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.transform.gameObject != null)
                {
                    
                }
            }
        }
    }
}
