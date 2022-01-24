using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Visitors : MonoBehaviour
{
    NavMeshAgent myNavMeshAgent;
    private bool hasDestination;
    private NavMeshPath path;
    private int target;
    public int NBR_ATTRACTION;

    void Start()
    {
        myNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        path = new NavMeshPath();
    }

    void Update()
    {
        if (!myNavMeshAgent.hasPath)
        {
            SetDestinationAttraction();
        }
        

    }

    void SetDestinationAttraction()
    {
        target = Random.Range(0, NBR_ATTRACTION);
        var gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        var newDestination = gm.getEntryLocation(target);

        //newDestination.y = Terrain.activeTerrain.SampleHeight(newDestination);

        myNavMeshAgent.SetDestination(newDestination);
    }
}
