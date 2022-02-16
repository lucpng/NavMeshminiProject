using UnityEngine;
using UnityEngine.AI;

public class Wanderers : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    private NavMeshPath path;

    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        path = new NavMeshPath();
    }

    void Update()
    {

        if (!navMeshAgent.hasPath)
        {
            for(uint i =0; i < 10; ++i)
            {
                if (SetDestinationRandom())
                {
                    break;
                };
            } 
        }
        
    }

    bool SetDestinationRandom()
    {
        Vector3 newDestination = new Vector3(Random.Range(0.0f, GameManager.terrainSize.x), 0.0f, Random.Range(0.0f, GameManager.terrainSize.y));
        NavMeshHit hit;
        NavMesh.SamplePosition(newDestination, out hit, float.PositiveInfinity, NavMesh.AllAreas);
        NavMesh.CalculatePath(this.transform.position, hit.position, NavMesh.AllAreas, path);
        if(path.status == NavMeshPathStatus.PathComplete)
        {
            navMeshAgent.SetDestination(newDestination);
            return true;
        }
        return false;
        
    }
}
