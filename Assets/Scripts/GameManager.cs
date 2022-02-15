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

    public int nbWanderers = 10;
    public int nbVisitors = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Vector3 spawnPosition = new Vector3(4.7f, 1.4f, 3.65f);
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
            if (NavMesh.SamplePosition(randomPosition, out hit, float.PositiveInfinity, 1))
                return hit.position;
        }

        return Vector3.zero;
    }
}



