using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject visitor;
    public GameObject patroller;
    public GameObject attractionList;
    public GameObject agentList;

    public int nbPatrollers = 10;
    public int nbVisitors = 0;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 spawnPosition = new Vector3(4.7f, 1.4f, 3.65f);
        for (int i=0; i< nbPatrollers; ++i)
        {
            GameObject newAgent = Instantiate(patroller, spawnPosition, Quaternion.identity);
            newAgent.transform.SetParent(agentList.transform);
        }
        for(int i=0; i< nbVisitors; ++i)
        {
            GameObject newAgent = Instantiate(visitor, spawnPosition, Quaternion.identity);
            newAgent.transform.SetParent(agentList.transform);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
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
}
