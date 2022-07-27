using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    enum EnemyState { Patrol = 0, Investigate = 1 }

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PatrolRoute patrolRoute;
    [SerializeField] private FieldOfView fov;
    [SerializeField] private EnemyState state = EnemyState.Patrol;
    [SerializeField] private float threshold = 0.5f;
    [SerializeField] private float waitTime = 2f;

    private Transform _currentPoint;
    private Vector3 _investigationPoint;
    private float _waitTimer;
    private int _routeIndex;
    private bool _moving;
    private bool _forwardsAlongPath = true;

    private void Start()
    {
        _currentPoint = patrolRoute.route[_routeIndex];
    }

    private void Update()
    {
        if (fov.visibleObjects.Count > 0)
        {
            InvestigatePoint(fov.visibleObjects[0].position);
        }

        if (state == EnemyState.Patrol)
        {
            UpdatePatrol();
        }
        else if (state == EnemyState.Investigate)
        {
            UpdateInvestigate();
        }
    }

    private void NextPatrolPoint()
    {
        if (_forwardsAlongPath) _routeIndex++;
        else _routeIndex--;

        if (_routeIndex == patrolRoute.route.Count)
        {
            if (patrolRoute.patrolType == PatrolRoute.PatrolType.Loop)
                _routeIndex = 0;
            else
            {
                _forwardsAlongPath = false;
                _routeIndex -= 2;
            }
        }

        if (_routeIndex == 0) _forwardsAlongPath = true;
        _currentPoint = patrolRoute.route[_routeIndex];
    }

    public void InvestigatePoint(Vector3 investigatePoint)
    {
        state = EnemyState.Investigate;
        _investigationPoint = investigatePoint;
        agent.SetDestination(_investigationPoint);
    }

    private void UpdateInvestigate()
    {
        if (Vector3.Distance(transform.position, _investigationPoint) < threshold)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer > waitTime) 
            { ReturnToPatrol(); }
        }
    }

    private void ReturnToPatrol()
    {
        state = EnemyState.Patrol;
        _waitTimer = 0f;
        _moving = false;
    }

    private void UpdatePatrol()
    {
        if (!_moving)
        {
            NextPatrolPoint();
            agent.SetDestination(_currentPoint.position);
            _moving = true;
        }

        if (_moving && Vector3.Distance(transform.position, _currentPoint.position) < threshold) 
        { _moving = false; }
    }
}
