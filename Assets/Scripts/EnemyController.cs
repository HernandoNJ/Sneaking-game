using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _threshold = 0.5f;
    [SerializeField] private PatrolRoute _patrolRoute;

    private Transform _currentPoint;
    private int _routeIndex;
    private bool _moving;    
    private bool _forwardsAlongPath = true;    

    private void Start()
    {
        _currentPoint = _patrolRoute.route[_routeIndex];
    }

    private void Update()
    {
        if (!_moving)
        {
            NextPatrolPoint();
            _agent.SetDestination(_currentPoint.position);
            _moving = true;
        }

        if (_moving && Vector3.Distance(transform.position, _currentPoint.position) < _threshold)
        {
            _moving = false;
        }
    }

    private void NextPatrolPoint()
    {
        if (_forwardsAlongPath) _routeIndex++;
        else _routeIndex--;

        if (_routeIndex == _patrolRoute.route.Count)
        {
            if (_patrolRoute.patrolType == PatrolRoute.PatrolType.Loop) 
                _routeIndex = 0;
            else 
            {
                _forwardsAlongPath = false;
                _routeIndex-=2;
            }
        }

        if (_routeIndex == 0) _forwardsAlongPath = true;
        _currentPoint = _patrolRoute.route[_routeIndex];
    }
}
