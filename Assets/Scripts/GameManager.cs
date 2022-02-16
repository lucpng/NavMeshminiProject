using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject visitor;
    public GameObject wanderer;
    public GameObject attractionList;
    public GameObject agentList;
    public List<Visitors> allVisitors;
    public List<Wanderers> allWanderers;
    public GameObject textToUpdate;
    public static Vector2 terrainSize = new Vector2(200, 200);
    private static Vector3 refOnNavMesh = new Vector3(151.2f, 21.7f, 105.02f);


    public int nbWanderers = 0;
    public int nbVisitors = 100;

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

        textToUpdate.GetComponent<TextUpdates>().UpdateText(allVisitors.Count.ToString(), allWanderers.Count.ToString()) ;
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

    public void AddVisitors(int amount)
    {
        for (uint i = 0; i < amount; ++i)
        {
            GameObject newAgent = Instantiate(visitor, getRandomSpawn(), Quaternion.identity);
            newAgent.transform.SetParent(agentList.transform);
            allVisitors.Add(newAgent.GetComponent<Visitors>());
        }
        textToUpdate.GetComponent<TextUpdates>().UpdateText(allVisitors.Count.ToString(), allWanderers.Count.ToString());
    }

    public void AddWanderers(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            GameObject newAgent = Instantiate(wanderer, getRandomSpawn(), Quaternion.identity);
            newAgent.transform.SetParent(agentList.transform);
            allWanderers.Add(newAgent.GetComponent<Wanderers>());
        }
        textToUpdate.GetComponent<TextUpdates>().UpdateText(allVisitors.Count.ToString(), allWanderers.Count.ToString());
    }

    public void RemoveVisitors(int amount)
    {
       
        if (allVisitors.Count > 0)
        {
            int nbIterations = Mathf.Min(amount, allVisitors.Count);
            for(int i=0 ; i < nbIterations; ++i)
            {
                allVisitors[i].GoingToBeDestroyed();
                Destroy(allVisitors[i].gameObject);
            }
            allVisitors.RemoveRange(0, nbIterations);
        }
        textToUpdate.GetComponent<TextUpdates>().UpdateText(allVisitors.Count.ToString(), allWanderers.Count.ToString());
    }

    public void RemoveWanderers(int amount)
    {

        if (allVisitors.Count > 0)
        {
            int nbIterations = Mathf.Min(amount, allWanderers.Count);
            for (int i = 0; i < nbIterations; ++i)
            {
                Destroy(allWanderers[i].gameObject);
            }
            allVisitors.RemoveRange(0, nbIterations);
        }
        textToUpdate.GetComponent<TextUpdates>().UpdateText(allVisitors.Count.ToString(), allWanderers.Count.ToString());
    }

    public int GetVisitorCount()
    {
        return allVisitors.Count;
    }
}



