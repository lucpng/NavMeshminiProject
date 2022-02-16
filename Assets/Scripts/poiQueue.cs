using System.Linq;
using System.Collections.Generic;


public class PoiQueue
{
    private List<Visitors> visitorsInQueue;
    private List<Visitors> comingVisitors;
    private Attraction attraction;

    public PoiQueue(Attraction owner)
    {
        visitorsInQueue = new List<Visitors>();
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

        visitorsInQueue.Add(visitor);
       
        Notify();
    }

    public Visitors GetFirst()
    {
        return visitorsInQueue[0];
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
            visitorsInQueue.RemoveAt(0);
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

    public void RemoveAndUpdateAllLists(Visitors visitor)
    {
        if (comingVisitors.Contains(visitor))
            comingVisitors.Remove(visitor);
        if (visitorsInQueue.Contains(visitor)){
            var newPreviousVisitor = visitor.previousVisitor;
            var nextVisitorId = visitorsInQueue.IndexOf(visitor)+1;
            if (nextVisitorId >= 0 && nextVisitorId < visitorsInQueue.Count)
            {
                visitorsInQueue[nextVisitorId].previousVisitor = newPreviousVisitor;
            }
            
            visitorsInQueue.Remove(visitor);
        }
        Notify();
    }
}
