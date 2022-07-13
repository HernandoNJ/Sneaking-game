using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private Color _gizmoColor =  Color.red;
    [SerializeField] private float _viewRadius = 6f;
    [SerializeField] private float _viewAngle = 30f;
    //[SerializeField] private Creature _creature;
    [SerializeField] private LayerMask _blockingLayers;
    
    public List<Transform> visibleObjects;
    //public Creature creature;

    private void Update()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, _viewRadius);

        foreach (var target in targets)
        {
            if(target.GetComponent<Creature>()) Debug.Log(target);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);
    }
}
