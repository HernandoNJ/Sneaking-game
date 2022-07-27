using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Transform holdPosition;
    [SerializeField] private float grabRange = 2f;
    [SerializeField] private float throwForce = 20f;
    [SerializeField] private float snapSpeed= 40f;

   [SerializeField] private Rigidbody grabbedObject;
    private bool _grabPressed;

    private void FixedUpdate()
    {
        if (grabbedObject)
        {
            grabbedObject.velocity = (holdPosition.position - grabbedObject.transform.position) * snapSpeed;
        }
    }

    private void OnGrab()
    {
        if (_grabPressed)
        {
            _grabPressed = false;
            if (!grabbedObject) return;
            
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
        if (Physics.Raycast(cameraPosition.position, cameraPosition.forward, out RaycastHit hit, grabRange))
        {
            if (!hit.transform.gameObject.CompareTag("Grabbable")) return;

            grabbedObject = hit.transform.GetComponent<Rigidbody>();
            grabbedObject.transform.SetParent(holdPosition);
        }
    }

    private void DropGrabbedObject()
    {
        grabbedObject.transform.parent = null;
        grabbedObject = null;
    }

    private void OnThrow()
    {
        if (!grabbedObject) return;
        
        grabbedObject.AddForce(cameraPosition.forward *throwForce, ForceMode.Impulse);
        
        DropGrabbedObject();
    }
}
