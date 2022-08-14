using UnityEngine;

public class MinimapRotator : MonoBehaviour
{
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform RightHandTransform;
    [SerializeField] private Transform rotationReference;

    private Vector3 _initialRotation;
    private Handedness _handedness;

    private void Start()
    {
        _initialRotation = transform.eulerAngles;
        _handedness = GetComponent<Handedness>();

        if (_handedness.handed == Handed.Left)
        {
            rotationReference = RightHandTransform;
        }
        else
        {
            rotationReference = leftHandTransform;
        }
    }

    private void Update()
    {
        Vector3 newRot = new Vector3(0, 0, -rotationReference.eulerAngles.y) + _initialRotation;
        transform.rotation = Quaternion.Euler(newRot);
    }
}
