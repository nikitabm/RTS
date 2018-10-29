using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomMoveCommand
{

    public List<int> units = new List<int>();
    public Vector3 pos;
    public CustomMoveCommand(List<int> pUnits, Vector3 pPos)
    {
        units = pUnits;
		pos=pPos;
    }
	public CustomMoveCommand()
	{
		
	}

}
