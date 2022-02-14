using UnityEngine;
using UnityEngine.AI;

public class Visitors : MonoBehaviour
{
    public enum visitorStates
    {
        walking,
        atAttractionEntry,
        inQueue,
        inAttraction,
    }
    public visitorStates state;
    NavMeshAgent myNavMeshAgent;
    private bool hasDestination;
    private NavMeshPath path;
    private int target;
    private float attractionTime;
    public int NBR_ATTRACTION;
    private bool observingQueue = false;
    public Visitors previousVisitor;


    void Start()
    {
        myNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        SetNewDestination();
        state = visitorStates.walking;
        path = new NavMeshPath();
    }

    void Update()
    {
        var gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        var attraction = gm.GetAttractionById(target);
        switch (state)
        {
            case visitorStates.walking:
                if (myNavMeshAgent.pathPending)
                    break;
                //Debug.Log("Distance to objectif = " + myNavMeshAgent.remainingDistance);
                if (myNavMeshAgent.remainingDistance < 1)
                {
                   
                    EnqueVisitor();
                    state = visitorStates.inQueue;
                }
                break;
            case visitorStates.atAttractionEntry:
                observingQueue = false;
                if (!gm.AttractionIsFull(target))
                {
                    EnterAttraction();
                    state = visitorStates.inAttraction;
                }
                break;
            case visitorStates.inQueue:
                if (previousVisitor)
                {
                    if (previousVisitor.state == visitorStates.inAttraction)
                    {
                        var attractionEntry = attraction.Find("Entry");
                        myNavMeshAgent.isStopped = false;
                        myNavMeshAgent.SetDestination(attractionEntry.transform.position);
                        previousVisitor = null;
                    }
                    else if (myNavMeshAgent.remainingDistance < 1)
                    {
                        myNavMeshAgent.isStopped = true;
                    }
                    else
                    {
                        myNavMeshAgent.isStopped = false;
                        myNavMeshAgent.SetDestination(previousVisitor.transform.position);
                    }
                }
                else
                {
                    state = visitorStates.atAttractionEntry;
                }
                break;
            case visitorStates.inAttraction:
                attractionTime -= Time.deltaTime;
                if (attractionTime <= 0)
                {
                    ExitAttraction();
                    state = visitorStates.walking;
                }         
                break;

        }
        
    }

    void SetNewDestination()
    {
        target = Random.Range(0, NBR_ATTRACTION);
        var gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        var attraction = gm.GetAttractionById(target);
        observingQueue = true;
        if (!attraction.GetComponent<Attraction>().QueueIsEmpty())
        {
            myNavMeshAgent.SetDestination(attraction.GetComponent<Attraction>().LastPosInQueue());
        }
        else
        {
            var attractionEntry = attraction.Find("Entry");
            if (attractionEntry != null)
            {
                myNavMeshAgent.SetDestination(attractionEntry.transform.position);
            }
        }
        attraction.GetComponent<Attraction>().AddVisitorArriving(this);
    }

    void EnterAttraction()
    {
        var gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        var attraction = gm.GetAttractionById(target);
        if (attraction != null)
        {
            myNavMeshAgent.GetComponent<MeshRenderer>().enabled = false;
            myNavMeshAgent.GetComponent<NavMeshAgent>().enabled = false;
            attractionTime = attraction.GetComponent<Attraction>().duration;
            attraction.GetComponent<Attraction>().IncrementUsersAttraction();
        }
    }

    void ExitAttraction()
    {
        var gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        var attraction = gm.GetAttractionById(target);
        var attractionExit = attraction.Find("Exit");
        if (attractionExit != null)
        {
            myNavMeshAgent.transform.position = attractionExit.transform.position;
            myNavMeshAgent.GetComponent<MeshRenderer>().enabled = true;
            myNavMeshAgent.GetComponent<NavMeshAgent>().enabled = true;
            SetNewDestination();
            attraction.GetComponent<Attraction>().DecrementUsersAttraction();
        }

    }
    void EnqueVisitor()
    {
        var gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        var attraction = gm.GetAttractionById(target);
        attraction.GetComponent<Attraction>().Enqueue(this);
    }

    public void UpdatesAboutQueue(Vector3 newLastPosInQueue)
    {
        if (observingQueue)
        {
            myNavMeshAgent.SetDestination(newLastPosInQueue);
        }
    }

    public void SetPreviousVisitor(Visitors visitor){
        previousVisitor = visitor;
    }
}
