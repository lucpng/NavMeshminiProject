using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrollers : MonoBehaviour
{
    NavMeshAgent myNavMeshAgent;
    private bool hasDestination;
    private NavMeshPath path;
    public int mode;

    void Start()
    {
        myNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            SetDestinationToMousePosition();
        }*/
        if (!myNavMeshAgent.hasPath)
        {
            SetDestinationRandom();
        }
        

    }

    void SetDestinationRandom()
    {
        Vector3 newDestination = new Vector3(Random.Range(0.0f, 95.0f), 0.0f, Random.Range(0.0f, 95.0f));
        newDestination.y = Terrain.activeTerrain.SampleHeight(newDestination);
        //Debug.Log("newDestination : " + newDestination);
        myNavMeshAgent.SetDestination(newDestination);
    }

    void SetDestinationToMousePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            myNavMeshAgent.SetDestination(hit.point);
        }
    }
}
