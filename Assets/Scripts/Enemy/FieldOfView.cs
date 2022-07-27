using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private Color gizmoColor = Color.red;
    [SerializeField] private float viewRadius = 6f;
    [SerializeField] private float viewAngle = 30f;
    [SerializeField] private Creature creature;
    [SerializeField] private LayerMask blockingLayers;

    public List<Transform> visibleObjects;
    //public Creature creature;

    private void Update()
    {
        visibleObjects.Clear();
        
        Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius);

        foreach (var target in targets)
        {
            if (!target.TryGetComponent(out Creature targetCreature))
                continue;

            if (creature.team == targetCreature.team) continue;
            
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle)
            {
                Vector3 headPos = creature.head.position;
                Vector3 targetHeadPos = targetCreature.head.transform.position;

                Vector3 dirToTargetHead = (targetHeadPos - headPos).normalized;
                float distToTargetHead = Vector3.Distance(headPos, targetHeadPos);

                if (Physics.Raycast(headPos, dirToTargetHead, distToTargetHead, blockingLayers))
                    continue;

                Debug.DrawLine(headPos, targetHeadPos, Color.magenta);
                visibleObjects.Add(target.transform);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Handles.color = gizmoColor;

        Handles.DrawWireArc(transform.position, transform.up, transform.forward, viewAngle, viewRadius);
        Handles.DrawWireArc(transform.position, transform.up, transform.forward, -viewAngle, viewRadius);
        
        Vector3 lineA = Quaternion.AngleAxis(viewAngle, Vector3.up) * transform.forward;
        Vector3 lineB = Quaternion.AngleAxis(-viewAngle, Vector3.up) * transform.forward;
        Handles.DrawLine(transform.position, transform.position + lineA * viewRadius);
        Handles.DrawLine(transform.position, transform.position + lineB * viewRadius);
    }

}
