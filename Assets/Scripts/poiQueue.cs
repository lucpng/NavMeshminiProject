using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poiQueue
{
    Queue<Visitors> visitorsInQueue;
    List<Visitors> comingVisitors;
    Visitors last;
    Attraction attraction;

    public poiQueue(Attraction owner)
    {
        visitorsInQueue = new Queue<Visitors>();
        comingVisitors = new List<Visitors>();
        attraction = owner;
    }

    public void AddComing(Visitors visitor)
    {
        if(!comingVisitors.Contains(visitor))
            comingVisitors.Add(visitor);
    }

    public void EnqueueArrived(Visitors visitor)
    {

        comingVisitors.Remove(visitor);
        visitorsInQueue.Enqueue(visitor);
        if (last)
        {
            visitor.SetPreviousVisitor(last);
        }
        
        last = visitor;
        Notify();
    }

    private void Notify()
    {
        if (visitorsInQueue.Count > 0)
        {
            foreach (Visitors v in comingVisitors)
            {
                v.setDestination(last.transform.position);
            }
        }
        else
        {
            foreach (Visitors v in comingVisitors)
            {
                v.setDestination(attraction.entrance.transform.position);
            }

        }
    }

    public Vector3 LastPosInQueue()
    {
        return last.transform.position;
    }

    public void ReleaseFirst()
    {
        if (!QueueIsEmpty())
            visitorsInQueue.Dequeue();
    }

    public bool QueueIsEmpty()
    {
        if (visitorsInQueue.Count > 0)
        {
            
            return false;
        }
        else
        {
            return true;
        } 
    }

    public Visitors Peek()
    {
        return visitorsInQueue.Peek();
    }
}
