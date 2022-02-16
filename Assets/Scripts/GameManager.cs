using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public GameObject visitor;
    public GameObject wanderer;
    public GameObject attractionList;
    public GameObject agentList;
    public List<Visitors> allVisitors;
    public List<Wanderers> allWanderers;
    public static Vector2 terrainSize = new Vector2(200, 200);
    private static Vector3 refOnNavMesh = new Vector3(151.2f, 21.7f, 105.02f);


    public int nbWanderers = 10;
    public int nbVisitors = 0;

    void Start()
    {
        for (int i=0; i< nbWanderers; ++i)
        {
            GameObject newAgent = Instantiate(wanderer, getRandomSpawn(), Quaternion.identity);
            newAgent.transform.SetParent(agentList.transform);
            allWanderers.Add(newAgent.GetComponent<Wanderers>());
        }
        for(int i=0; i< nbVisitors; ++i)
        {
            GameObject newAgent = Instantiate(visitor, getRandomSpawn(), Quaternion.identity);
            newAgent.transform.SetParent(agentList.transform);           
            allVisitors.Add(newAgent.GetComponent<Visitors>());
        }

    }

    public Transform GetAttractionById(int index)
    {
        return attractionList.transform.GetChild(index);
    }

    public bool AttractionIsFull(int index)
    {
        var target = attractionList.transform.GetChild(index);
        return target.GetComponent<Attraction>().isFull();
    }

    private Vector3 getRandomSpawn()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(0, terrainSize.x), 0, Random.Range(0, terrainSize.y));
            NavMeshHit hit;
            NavMeshHit hitRef;
            NavMeshPath path = new NavMeshPath();
            var posOnNavMesh = NavMesh.SamplePosition(randomPosition, out hit, float.PositiveInfinity, NavMesh.AllAreas);
            NavMesh.SamplePosition(refOnNavMesh, out hitRef, float.PositiveInfinity, NavMesh.AllAreas);
            NavMesh.CalculatePath(hit.position, hitRef.position, NavMesh.AllAreas, path);
            if (posOnNavMesh)
            {
                if(path.status == NavMeshPathStatus.PathComplete)
                {
                    return hit.position;
                }
            }
                
        }

        return Vector3.zero;
    }
}



