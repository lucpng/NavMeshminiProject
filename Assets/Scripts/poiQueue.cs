using System.Linq;
using System.Collections.Generic;


public class poiQueue
{
    private Queue<Visitors> visitorsInQueue;
    private List<Visitors> comingVisitors;
    private Attraction attraction;

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

        if(visitorsInQueue.Count > 0)
        {
            visitor.SetPreviousVisitor(visitorsInQueue.Last());
        }
        else
        {
            visitor.SetDestination(attraction.entrance.transform.position);
        }
    }

    public void EnqueueArrived(Visitors visitor)
    {
        if (comingVisitors.Contains(visitor))
            comingVisitors.Remove(visitor);

        visitorsInQueue.Enqueue(visitor);
       
        Notify();
    }

    private void Notify()
    {
        if (visitorsInQueue.Count > 0)
        {
            foreach (Visitors v in comingVisitors)
            {
                v.SetPreviousVisitor(visitorsInQueue.Last());
            }
        }
        else
        {
            foreach (Visitors v in comingVisitors)
            {
                v.SetDestination(attraction.entrance.transform.position);
                v.SetPreviousVisitor(null);
            }

        }
    }

    public void ReleaseFirst()
    {
        if (!QueueIsEmpty())
            visitorsInQueue.Dequeue();
        Notify();
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
