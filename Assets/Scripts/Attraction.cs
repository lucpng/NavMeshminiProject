using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attraction : MonoBehaviour
{
    public float duration;
    public int capacity = 1;
    public int users = 0;
    public bool full;
    agentsQueue queue;

    // Start is called before the first frame update
    void Start()
    {
        full = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(users >= capacity)
        {
            full = true;
        }
        else
        {
            full = false;
        }
    }

    public void AddVisitorArriving(Visitors visitor)
    {
        queue.AddArriving(visitor);
    }

    public void IncrementUsersAttraction()
    {
        ++users;
    }

    public void DecrementUsersAttraction()
    {
        if (users > 0)
            --users;  
    }

    public Vector3 LastPosInQueue()
    {
        return queue.LastPosInQueue();
    }
}
