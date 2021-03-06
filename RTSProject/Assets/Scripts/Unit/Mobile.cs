﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Mobile : MonoBehaviour
{


    public Vector3 target;
    private float _currentSpeed;
    private float _speedValue = 5.0f;
    public Vector3[] path;
    private int targetIndex;

    public bool stopMoving;
    private Grid grid;

    public enum MoveFSM
    {
        findPosition,
        recalculatePath,
        move,
        turnToFace,
        interact
    }

    public MoveFSM moveFSM;

    void Start()
    {
        _currentSpeed = _speedValue;
        grid = GameObject.FindGameObjectWithTag("A*").GetComponent<Grid>();
        CommandManager.GamePaused += ToggleSpeed;
    }
    void ToggleSpeed()
    {
        if (ServiceLocator.GetService<GameManager>().GamePaused) _currentSpeed = 0;
        else _currentSpeed = _speedValue;
    }
    void Update()
    {
        MoveStates();
    }

    public void MoveStates()
    {
        switch (moveFSM)
        {
            case MoveFSM.findPosition:

                break;
            case MoveFSM.recalculatePath:
                {
                    Node targetNode = grid.NodeFromWorldPoint(target);
                    if (targetNode.walkable == false)
                    {
                        stopMoving = false;
                        FindClosestWalkableNode(targetNode);
                        moveFSM = MoveFSM.move;
                    }
                    else if (targetNode.walkable == true)
                    {
                        Debug.Log("target node is walkable");
                        stopMoving = false;
                        PathRequestManager.RequestPath(transform.position, target, OnPathFound);
                        moveFSM = MoveFSM.move;
                    }
                }
                break;
            case MoveFSM.move:
                Move();
                break;
            case MoveFSM.turnToFace:
                //TurnToFace();
                break;
            case MoveFSM.interact:
                // if(currentInteractable != null)
                //currentInteractable.GetComponent<Interactable>().Interact(this.gameObject);
                break;
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            //todo: i added this path length check if sth is breaking maybe its better to delete it
            if (newPath.Length == 0) return;
            path = newPath;
            targetIndex = 0;
            RemoveUnitFromUnitManagerMovingUnitsList();
            UnitManager.instance.movingUnits.Add(this.gameObject);

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
            moveFSM = MoveFSM.move;

        }
    }

    private void FindClosestWalkableNode(Node originalNode)
    {
        Node comparisonNode = grid.grid[0, 0];
        Node incrementedNode = originalNode;
        for (int x = 0; x < incrementedNode.gridX; x++)
        {
            // Debug.Log("x: " + incrementedNode.gridX + " incremented node - 1: " + (incrementedNode.gridX - 1));
            incrementedNode = grid.grid[incrementedNode.gridX - 1, incrementedNode.gridY];

            if (incrementedNode.walkable == true)
            {
                comparisonNode = incrementedNode;
                target = comparisonNode.nodeWorldPosition;
                PathRequestManager.RequestPath(transform.position, target, OnPathFound);
                moveFSM = MoveFSM.move;
                break;
            }
        }

    }

    public void Move()
    {

    }

    public void SetWalkabilityOfCurrentNode(bool value)
    {
        Node myNode = grid.NodeFromWorldPoint(transform.position);
        myNode.walkable = value;
    }
    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    //TODO: make it walkable again
                    SetWalkabilityOfCurrentNode(false);
                    yield break;
                }
                else if (stopMoving == true)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, _currentSpeed * Time.deltaTime);
            yield return null;

        }
    }
    public void RequestPath(Vector3 point)
    {
        target = point;
        RemoveUnitFromUnitManagerMovingUnitsList();
        PathRequestManager.RequestPath(transform.position, target, OnPathFound);
    }

    private void RemoveUnitFromUnitManagerMovingUnitsList()
    {
        if (UnitManager.instance.movingUnits.Count > 0)
        {
            for (int i = 0; i < UnitManager.instance.movingUnits.Count; i++)
            {
                if (this.gameObject == UnitManager.instance.movingUnits[i])
                {
                    UnitManager.instance.movingUnits.Remove(UnitManager.instance.movingUnits[i]);
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
