using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsQueue
{
    Queue<Visitors> visitorsInQueue;
    List<Visitors> visitorsArriving;
    Visitors last;

    public AgentsQueue()
    {
        visitorsInQueue = new Queue<Visitors>();
        visitorsArriving = new List<Visitors>();
    }

    public void AddArriving(Visitors visitor)
    {
        Debug.Log("Bonjour");
        visitorsArriving.Add(visitor);
    }

    public void EnqueueArrived(Visitors visitor)
    {
        visitorsArriving.Remove(visitor);
        visitorsInQueue.Enqueue(visitor);
        if (last)
        {
            visitor.SetVisitorBefore(last);
        }
        
        last = visitor;
        Notify();
    }

    private void Notify()
    {
        visitorsArriving.ForEach(visitor => visitor.UpdatesAboutQueue(last.transform.position));
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
