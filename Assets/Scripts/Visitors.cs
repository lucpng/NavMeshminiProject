using UnityEngine;
using UnityEngine.AI;

public class Visitors : MonoBehaviour
{
    public enum visitorStates
    {
        walking,
        atAttractionEntrance,
        inQueue,
        inAttraction,
    }
    public visitorStates state;
    NavMeshAgent myNavMeshAgent;
    private Attraction targetedAttraction;
    private float attractionTime;
    public static int NBR_ATTRACTION = 4;
    private bool observingQueue = false;
    public Visitors previousVisitor;


    void Start()
    {
        myNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        SetNewDestination();
        state = visitorStates.walking;
    }

    void Update()
    {
        switch (state)
        {
            case visitorStates.walking:
                if (myNavMeshAgent.pathPending)
                    break;

                if (myNavMeshAgent.remainingDistance < 1)
                {
                   
                    EnqueVisitor();
                    state = visitorStates.inQueue;
                }
                break;
            case visitorStates.atAttractionEntrance:
                observingQueue = false;
                if (!targetedAttraction.full)
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
                        myNavMeshAgent.isStopped = false;
                        myNavMeshAgent.SetDestination(targetedAttraction.entrance.transform.position);
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
                    state = visitorStates.atAttractionEntrance;
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
        var targetID = Random.Range(0, NBR_ATTRACTION);
        var gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        targetedAttraction = gm.GetAttractionById(targetID).GetComponent<Attraction>();
        observingQueue = true;
        if (!targetedAttraction.QueueIsEmpty())
        {
            myNavMeshAgent.SetDestination(targetedAttraction.LastPosInQueue());
        }
        else
        {
            var attractionEntrance = targetedAttraction.entrance;
            if (attractionEntrance != null)
            {
                myNavMeshAgent.SetDestination(attractionEntrance.transform.position);
            }
        }
        targetedAttraction.AddComingVisitor(this);
    }

    void EnterAttraction()
    {
        if (targetedAttraction != null)
        {
            myNavMeshAgent.GetComponent<MeshRenderer>().enabled = false;
            myNavMeshAgent.GetComponent<NavMeshAgent>().enabled = false;
            attractionTime = targetedAttraction.duration;
            targetedAttraction.IncrementUsersAttraction();
        }
    }

    void ExitAttraction()
    {
        var attractionExit = targetedAttraction.exit;
        if (attractionExit != null)
        {
            myNavMeshAgent.transform.position = attractionExit.transform.position;
            myNavMeshAgent.GetComponent<MeshRenderer>().enabled = true;
            myNavMeshAgent.GetComponent<NavMeshAgent>().enabled = true;
            SetNewDestination();
            targetedAttraction.DecrementUsersAttraction();
        }

    }
    void EnqueVisitor() {targetedAttraction.Enqueue(this);}

    public void SetPreviousVisitor(Visitors visitor){
        previousVisitor = visitor;
    }

    public void setDestination(Vector3 newDest) { myNavMeshAgent.SetDestination(newDest); }
}
