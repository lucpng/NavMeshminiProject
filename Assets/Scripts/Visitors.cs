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




    void Start()
    {
        myNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        state = visitorStates.walking;
        path = new NavMeshPath();
    }

    void Update()
    {
        switch (state)
        {
            case visitorStates.walking:
                if (!myNavMeshAgent.hasPath)
                {
                    SetDestinationAttraction();
                }
                break;
            case visitorStates.atAttractionEntry:
                var gm = GameObject.Find("GameManager").GetComponent<GameManager>();            
                if (!gm.attractionIsFull(target))
                {
                    EnterAttraction();
                    state = visitorStates.inAttraction;
                }           
                break;
            case visitorStates.inAttraction:
                attractionTime -= Time.deltaTime;
                if (attractionTime <= 0)
                {
                    Debug.Log(attractionTime);
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
        if (attraction.GetComponent<Attraction>().full)
        {
            myNavMeshAgent.SetDestination(attraction.GetComponent<Attraction>().LastPosInQueue());
            observingQueue = true;
        }
        else
        {
            var attractionEntry = attraction.Find("Entry");
            if (attractionEntry != null)
            {
                myNavMeshAgent.SetDestination(attractionEntry.transform.position);
                attraction.GetComponent<Attraction>().AddVisitorArriving(this);
            }
        }
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

    public void updatesAboutQueue(Vector3 newLastPosInQueue)
    {
        myNavMeshAgent.SetDestination(newLastPosInQueue);
    }
}
