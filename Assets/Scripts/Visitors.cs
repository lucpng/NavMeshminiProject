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
        goingInQueue,
    }
    public visitorStates state;
    NavMeshAgent myNavMeshAgent;
    private bool hasDestination;
    private NavMeshPath path;
    private int target;
    private float attractionTime;
    public int NBR_ATTRACTION;
    //private bool observingQueue = false;
    public Visitors previousVisitor;


    void Start()
    {
        myNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
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
                if (!myNavMeshAgent.hasPath)
                {
                    SetDestinationAttraction();
                }
                if (myNavMeshAgent.remainingDistance < 1)
                {
                    state = visitorStates.atAttractionEntry;
                }
                break;
            case visitorStates.goingInQueue:
                if (myNavMeshAgent.remainingDistance < 2)
                {
                    myNavMeshAgent.isStopped = true;
                    EnqueVisitor();
                    state = visitorStates.inQueue;
                }
                break;
            case visitorStates.atAttractionEntry:
                if (!attraction.GetComponent<Attraction>().QueueIsEmpty()) // TODO HERE ________________
                {
                    if (!gm.AttractionIsFull(target) && this == attraction.GetComponent<Attraction>().getFirstInQueue())
                    {
                        EnterAttraction();
                        state = visitorStates.inAttraction;
                    }
                }
                else
                {
                    if (!gm.AttractionIsFull(target))
                    {
                        EnterAttraction();
                    }
                }
                break;

            case visitorStates.inQueue:
                if (previousVisitor)
                {
                    if (previousVisitor.state == visitorStates.inAttraction)
                    {
                        var attractionEntry = attraction.Find("Entry");
                        myNavMeshAgent.SetDestination(attractionEntry.transform.position);
                        previousVisitor = null;
                    }
                    else if (myNavMeshAgent.remainingDistance < 2)
                    {
                        myNavMeshAgent.isStopped = true;
                    }
                    else
                    {
                        myNavMeshAgent.SetDestination(previousVisitor.transform.position);
                    }
                }
                else
                {
                    if (myNavMeshAgent.remainingDistance < 1)
                    {
                        state = visitorStates.atAttractionEntry;
                    }
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

    void SetDestinationAttraction()
    {
        target = Random.Range(0, NBR_ATTRACTION);
        var gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        var attraction = gm.GetAttractionById(target);
        if (attraction.GetComponent<Attraction>().full && !attraction.GetComponent<Attraction>().QueueIsEmpty())
        {
            Debug.Log("Here");
            myNavMeshAgent.SetDestination(attraction.GetComponent<Attraction>().LastPosInQueue());
            state = visitorStates.goingInQueue;
            //observingQueue = true;
        }
        else
        {
            Debug.Log("There");
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
            SetDestinationAttraction();
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
        myNavMeshAgent.SetDestination(newLastPosInQueue);
        state = visitorStates.goingInQueue;
    }

    public void SetVisitorBefore(Visitors visitor){
        previousVisitor = visitor;
    }
}
