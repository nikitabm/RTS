using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SelectObject : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //ClickOnObjects();
    }
    private void ClickOnObjects()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.transform.gameObject != null)
                {
					Debug.Log(hit.transform.name);
                }
            }
        }
    }
}
