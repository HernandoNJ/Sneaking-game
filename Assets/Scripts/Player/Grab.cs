using System;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] private Transform _cameraPosition;
    [SerializeField] private Transform _holdPosition;
    [SerializeField] private float _grabRange = 2f;
    [SerializeField] private float _throwForce = 20f;
    [SerializeField] private float _snapSpeed= 40f;

   [SerializeField] private Rigidbody _grabbedObject;
    private bool _grabPressed;

    private void FixedUpdate()
    {
        if (_grabbedObject)
        {
            _grabbedObject.velocity = (_holdPosition.position - _grabbedObject.transform.position) * _snapSpeed;
        }
    }

    private void OnGrab()
    {
        if (_grabPressed)
        {
            _grabPressed = false;
            if (!_grabbedObject) return;
            
            DropGrabbedObject();
        }
        else
        {
            _grabPressed = true;

            CheckRaycastToGrabbable();
        }
    }

    private void CheckRaycastToGrabbable()
    {
        if (Physics.Raycast(_cameraPosition.position, _cameraPosition.forward, out RaycastHit hit, _grabRange))
        {
            if (!hit.transform.gameObject.CompareTag("Grabbable")) return;

            _grabbedObject = hit.transform.GetComponent<Rigidbody>();
            _grabbedObject.transform.SetParent(_holdPosition);
        }
    }

    private void DropGrabbedObject()
    {
        _grabbedObject.transform.parent = null;
        _grabbedObject = null;
    }

    private void OnThrow()
    {
        if (!_grabbedObject) return;
        
        _grabbedObject.AddForce(_cameraPosition.forward *_throwForce, ForceMode.Impulse);
        
        DropGrabbedObject();
    }
}

// _grabPressed = !_grabPressed;
// Debug.Log(_grabPressed?"Grabbing":"releasing");