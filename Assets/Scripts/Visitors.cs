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
    NavMeshAgent navMeshAgent;
    private Attraction targetedAttraction;
    private float attractionTime;
    public static int NBR_ATTRACTION = 4;
    public Visitors previousVisitor;


    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        SetNewDestination();
        state = visitorStates.walking;
    }

    void Update()
    {
        switch (state)
        {
            case visitorStates.walking:
                if (navMeshAgent.pathPending)
                    break;
                
                if (DistanceToTarget(navMeshAgent.destination) < 2.0f)
                {
                    EnqueVisitor();
                    state = visitorStates.inQueue;
                }
                break;
            case visitorStates.atAttractionEntrance:
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
                        previousVisitor = null;
                        navMeshAgent.SetDestination(targetedAttraction.entrance.transform.position);
                        navMeshAgent.isStopped = false; 
                    }
                    else if (DistanceToTarget(previousVisitor.transform.position) < 4.0f)
                    {
                        navMeshAgent.isStopped = true;
                    }
                    else
                    {
                        navMeshAgent.isStopped = false;
                        navMeshAgent.SetDestination(previousVisitor.transform.position);
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
        targetedAttraction.AddComingVisitor(this);
    }

    void EnterAttraction()
    {
        if (targetedAttraction != null)
        {
            navMeshAgent.GetComponent<MeshRenderer>().enabled = false;
            navMeshAgent.GetComponent<NavMeshAgent>().enabled = false;
            attractionTime = targetedAttraction.duration;
            targetedAttraction.IncrementUsersAttraction();
        }
    }

    void ExitAttraction()
    {
        navMeshAgent.transform.position = targetedAttraction.exit.transform.position;
        navMeshAgent.GetComponent<MeshRenderer>().enabled = true;
        navMeshAgent.GetComponent<NavMeshAgent>().enabled = true;
        SetNewDestination();
        targetedAttraction.DecrementUsersAttraction();

    }
    void EnqueVisitor() {targetedAttraction.Enqueue(this);}

    public void SetPreviousVisitor(Visitors visitor){
        previousVisitor = visitor;
        if (previousVisitor)
        {
            SetDestination(visitor.transform.position);
        }  
    }

    public void SetDestination(Vector3 newDest) { navMeshAgent.SetDestination(newDest); }

    public float DistanceToTarget(Vector3 targetPosition)
    {
        return (targetPosition - this.transform.position).sqrMagnitude;
    }
}
