using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private float threshold = 0.5f;

    private Transform currentPoint;
    private bool moving = false;

    private void Update()
    {
        if (!moving)
        {
            if (currentPoint == point1)
            {
                currentPoint = point2;
            }
            else
            {
                currentPoint = point1;
            }
            
            agent.SetDestination(currentPoint.position);
            moving = true;
        }

        if (moving && Vector3.Distance(transform.position, currentPoint.position) < threshold)
        {
            moving = false;
        }
    }
}
