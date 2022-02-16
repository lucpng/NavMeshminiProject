using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attraction : MonoBehaviour
{
    [Range(0.5f, 20f)]
    public int duration = 1 ;
    [Range(1, 10)]
    public int capacity = 1;
    private int users = 0;
    public bool full = false;
    public PoiQueue queue;
    public GameObject entrance;
    public GameObject exit;

    void Start()
    {
        queue = new PoiQueue(this);
    }

    public void AddComingVisitor(Visitors visitor)
    {
        
        queue.AddComing(visitor);
    }

    public void Enqueue(Visitors visitor)
    {
        queue.EnqueueArrived(visitor);
    }

    public bool isFull(){
        if(users >= capacity)
        {
            full = true;
        }
        else
        {
            full = false;
        }
        return full;
    }
    public void IncrementUsersAttraction()
    {        
        ++users;
        queue.ReleaseFirst();
        isFull();  
    }

    public void DecrementUsersAttraction()
    {
        if (users > 0)
            --users;
        isFull();
    }

    public bool QueueIsEmpty()
    {
        return queue.QueueIsEmpty();
    }

    public void UpdateQueueAboutDestroyedAgent(Visitors visitor)
    {
        queue.RemoveAndUpdateAllLists(visitor);
    }

    public bool AmIFirst(Visitors visitor)
    {
        return visitor == queue.GetFirst();
    }
}
