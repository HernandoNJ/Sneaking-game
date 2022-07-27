using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    [SerializeField] private float soundRadius = 5f;
    [SerializeField] private float checkRadius = 5f;
    [SerializeField] private float impulseThreshold = 2f;

    private float _collisionTimer;

    private AudioSource _audioSource;
    public UnityEvent onEmitSound;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        var mag = other.impulse.magnitude;
        var isPlayer = other.gameObject.CompareTag("Player");

        if (mag > impulseThreshold || isPlayer)
        {
            _audioSource.Play();

            Collider[] colliders = Physics.OverlapSphere(transform.position, soundRadius);

            foreach (var col in colliders)
            {
                if (col.gameObject.CompareTag("Geometry"))
                {
                    var newPos = colliders[0].transform.position;
                    gameObject.transform.SetParent(col.transform);
                }

                if (col.TryGetComponent(out EnemyController enemy))
                {
                    enemy.InvestigatePoint(transform.position);
                }
            }
        }
    }

    private void CheckIfEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadius);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, soundRadius);
    }
}
