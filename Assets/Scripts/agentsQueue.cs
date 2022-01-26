using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agentsQueue : MonoBehaviour
{
    Queue<Visitors> visitorsInQueue;
    List<Visitors> visitorsArriving;
    Visitors last;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddArriving(Visitors visitor)
    {
        visitorsArriving.Add(visitor);
    }

    public void enQueueArrived(Visitors visitor)
    {
        visitorsArriving.Remove(visitor);
        visitorsInQueue.Enqueue(visitor);
        last = visitor;
        notify();
    }

    private void notify()
    {
        visitorsArriving.ForEach(visitor => visitor.updatesAboutQueue(last.transform.position));
    }

    public Vector3 LastPosInQueue()
    {
        return last.transform.position;
    }
}
