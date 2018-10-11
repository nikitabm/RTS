using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour,Service {

	public delegate void SelectUnitAction();
	public static  event  SelectUnitAction OnSelected;
	void Start () {
		
	}
	
	void Update () {
	}
}
