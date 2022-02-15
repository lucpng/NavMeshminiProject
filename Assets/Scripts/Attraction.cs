using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attraction : MonoBehaviour
{
    public float duration;
    public int capacity = 1;
    public int users = 0;
    public bool full = false;
    public poiQueue queue;
    public GameObject entrance;
    public GameObject exit;

    void Start()
    {
        queue = new poiQueue(this);
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
        full = false;
    }

    public Vector3 LastPosInQueue()
    {
        return queue.LastPosInQueue();
    }

    public Visitors getFirstInQueue()
    {
        return queue.Peek();
    }

    public bool QueueIsEmpty()
    {
        return queue.QueueIsEmpty();
    }
}
