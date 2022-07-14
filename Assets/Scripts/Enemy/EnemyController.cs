using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    private enum EnemyState
    {
        Patrol = 0,
        Investigate = 1
    }

    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyState _state = EnemyState.Patrol;
    [SerializeField] private FieldOfView _fov;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private PatrolRoute _patrolRoute;

    [SerializeField] private float _threshold = 0.5f;
    [SerializeField] private float _waitTime = 2f;

    private Transform _currentPoint;
    private Vector3 _investigationPoint;

    private bool _moving;
    private bool _forwardsAlongPath = true;
    private bool _playerFound;
    private float _waitTimer;
    private int _routeIndex = 0;

    // [SerializeField] private Transform point1;
    // [SerializeField] private Transform point2;

    public UnityEvent<Transform> onPlayerFound;
    public UnityEvent onInvestigate;
    public UnityEvent onReturnToPatrol;

    private void Start()
    {
        _currentPoint = _patrolRoute.route[_routeIndex];
    }

    private void Update()
    {
        _animator.SetFloat("Speed", _agent.velocity.magnitude);

        if (_fov.visibleObjects.Count > 0) { PlayerFound(_fov.visibleObjects[0].position); }

        if (_state == EnemyState.Patrol) { UpdatePatrol(); }
        else if (_state == EnemyState.Investigate) { UpdateInvestigate(); }

        MoveRobot();
    }

    public void InvestigatePoint(Vector3 investigatePoint)
    {
        SetInvestigationPoint(investigatePoint);
        onInvestigate.Invoke();
    }

    private void SetInvestigationPoint(Vector3 investigatePoint)
    {
        _state = EnemyState.Investigate;
        _investigationPoint = investigatePoint;
        _agent.SetDestination(_investigationPoint);
    }

    private void PlayerFound(Vector3 investigatePoint)
    {
        //if (_playerFound) return;
        SetInvestigationPoint(investigatePoint);
        onPlayerFound.Invoke(_fov.creature._head);
        // onPlayerFound.Invoke(_fov.creature.head);
        //_playerFound = true;
    }

    private void UpdateInvestigate()
    {
        if (Vector3.Distance(transform.position, _investigationPoint) < _threshold)
        {
            _waitTimer += Time.deltaTime;

            if (_waitTimer > _waitTime) { ReturnToPatrol(); }
        }
    }

    private void ReturnToPatrol()
    {
        _state = EnemyState.Patrol;
        _waitTimer = 0f;
        _moving = false;

        onReturnToPatrol.Invoke();
    }

    private void UpdatePatrol()
    {
        if (!_moving)
        {
            NextPatrolPoint();
            _agent.SetDestination(_currentPoint.position);
            _moving = true;
        }

        if (_moving && Vector3.Distance(transform.position, _currentPoint.position) < _threshold) { _moving = false; }
    }

    private void MoveRobot()
    {
        if (!_moving)
        {
            // if (_currentPoint == point1) { _currentPoint = point2; }
            // else { _currentPoint = point1; }

            _agent.SetDestination(_currentPoint.position);
            _moving = true;
        }

        if (_moving && Vector3.Distance(transform.position, _currentPoint.position) < _threshold) { _moving = false; }
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
                _routeIndex -= 2;
            }
        }

        if (_routeIndex == 0) _forwardsAlongPath = true;
        _currentPoint = _patrolRoute.route[_routeIndex];
    }
}





// using UnityEngine;
// using UnityEngine.AI;
// using UnityEngine.Events;
//
// public class EnemyController : MonoBehaviour
// {
//     enum EnemyState { Patrol = 0, Investigate = 1 }
//
//     [SerializeField] private Animator _animator;
//     [SerializeField] private NavMeshAgent _agent;
//     [SerializeField] private PatrolRoute _patrolRoute;
//     [SerializeField] private FieldOfView _fov;
//     [SerializeField] private EnemyState _state = EnemyState.Patrol;
//     [SerializeField] private float _threshold = 0.5f;
//     [SerializeField] private float _waitTime = 2f;
//
//     private Transform _currentPoint;
//     private Vector3 _investigationPoint;
//     private float _waitTimer;
//     private int _routeIndex;
//     private bool _moving;
//     private bool _forwardsAlongPath = true;
//
//     private Transform currentPoint;
//     private bool moving;
//     private bool _playerFound;
//     
//     public UnityEvent<Transform> onPlayerFound;
//     public UnityEvent onInvestigate;
//     public UnityEvent onReturnToPatrol;
//     
//     private void Start()
//     {
//         _currentPoint = _patrolRoute.route[_routeIndex];
//     }
//
//     private void Update()
//     {
//         _animator.SetFloat("Speed", _agent.velocity.magnitude);
//         
//         if (_fov.visibleObjects.Count > 0)
//         { InvestigatePoint(_fov.visibleObjects[0].position); }
//         
//         if (_state == EnemyState.Patrol) 
//         { UpdatePatrol(); }
//         else if (_state == EnemyState.Investigate) 
//         { UpdateInvestigate(); }
//         
//         MoveRobot();
//     }
//
//     private void NextPatrolPoint()
//     {
//         if (_forwardsAlongPath) _routeIndex++;
//         else _routeIndex--;
//
//         if (_routeIndex == _patrolRoute.route.Count)
//         {
//             if (_patrolRoute.patrolType == PatrolRoute.PatrolType.Loop)
//                 _routeIndex = 0;
//             else
//             {
//                 _forwardsAlongPath = false;
//                 _routeIndex -= 2;
//             }
//         }
//
//         if (_routeIndex == 0) _forwardsAlongPath = true;
//         _currentPoint = _patrolRoute.route[_routeIndex];
//     }
//
//     public void InvestigatePoint(Vector3 investigatePoint)
//     {
//         _state = EnemyState.Investigate;
//         _investigationPoint = investigatePoint;
//         _agent.SetDestination(_investigationPoint);
//     }
//
//     private void UpdateInvestigate()
//     {
//         if (Vector3.Distance(transform.position, _investigationPoint) < _threshold)
//         {
//             _waitTimer += Time.deltaTime;
//             if (_waitTimer > _waitTime) 
//             { ReturnToPatrol(); }
//         }
//     }
//
//     private void ReturnToPatrol()
//     {
//         _state = EnemyState.Patrol;
//         _waitTimer = 0f;
//         _moving = false;
//     }
//
//     private void UpdatePatrol()
//     {
//         if (!_moving)
//         {
//             NextPatrolPoint();
//             _agent.SetDestination(_currentPoint.position);
//             _moving = true;
//         }
//
//         if (_moving && Vector3.Distance(transform.position, _currentPoint.position) < _threshold) 
//         { _moving = false; }
//     }
// }
