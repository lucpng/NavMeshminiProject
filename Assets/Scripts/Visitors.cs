using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

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
                if (!targetedAttraction.isFull() && targetedAttraction.AmIFirst(this))
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
                else if(targetedAttraction.AmIFirst(this))
                {
                    state = visitorStates.atAttractionEntrance;
                }
                else
                {
                    Debug.Log("DEBUG : I have no one in front of me but I am not first !");
                }
                break;
            case visitorStates.inAttraction:
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
            StartCoroutine(doingAttraction(targetedAttraction.duration));
            targetedAttraction.IncrementUsersAttraction();
        }
    }

    void ExitAttraction()
    {
        navMeshAgent.transform.position = targetedAttraction.exit.transform.position;
        navMeshAgent.GetComponent<MeshRenderer>().enabled = true;
        navMeshAgent.GetComponent<NavMeshAgent>().enabled = true;
        SetNewDestination();
        state = visitorStates.walking;
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

    public void GoingToBeDestroyed()
    {
        targetedAttraction.UpdateQueueAboutDestroyedAgent(this);
    }

    protected IEnumerator<WaitForSeconds> doingAttraction(int attractionTime)
    {
        // Function which will execute the lines under this "yield" after visitTime seconds
        yield return new WaitForSeconds(attractionTime);
        ExitAttraction();
    }
}
