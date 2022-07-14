using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    [SerializeField] private float _soundRadius = 5f;
    [SerializeField] private float _impulseThreshold = 2f;

    private float _collisionTimer;

    private AudioSource _audioSource;
    public UnityEvent onEmitSound;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        var mag = other.impulse.magnitude;
        var isPlayer = other.gameObject.CompareTag("Player");
        
        if (mag > _impulseThreshold || isPlayer)
        {
            _audioSource.Play();
            
            Collider[] colliders = Physics.OverlapSphere(transform.position, _soundRadius);

            foreach (var col in colliders)
            {
                if (col.TryGetComponent(out EnemyController enemy)) 
                { enemy.InvestigatePoint(transform.position); }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _soundRadius);
    }
}


// if(_collisionTimer < 2f) return;
//
// if (other.impulse.magnitude > _impulseThreshold || other.gameObject.CompareTag("Player"))
// {
//     _audioSource.Play();
//     onEmitSound.Invoke();
//     Debug.Log("Sound Emitter Collided with " + other.gameObject.name);
//     Collider[] _colliders = Physics.OverlapSphere(transform.position, _soundRadius);
//
//     foreach (var col in _colliders)
//     {
//         if (col.TryGetComponent(out EnemyController enemyController))
//         {
//             enemyController.InvestigatePoint(transform.position);
//         }
//     }
// }
// }
