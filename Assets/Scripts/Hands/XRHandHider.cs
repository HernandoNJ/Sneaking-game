using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Hands
{
public class XRHandHider : MonoBehaviour
{
    [SerializeField] private XRBaseControllerInteractor controller;
    [SerializeField] private Rigidbody handRigidbody;
    [SerializeField] private ConfigurableJoint configJoint;
    [SerializeField] private float handShowDelay = 0.015f;
    

    private Quaternion _originalHandRot;
    
    private void Start()
    {
        controller.selectEntered.AddListener(SelectEntered);
        controller.selectExited.AddListener(SelectExited);

        _originalHandRot = handRigidbody.transform.localRotation;
    }

    private void SelectEntered(SelectEnterEventArgs arg0)
    {
        if (arg0.interactable is BaseTeleportationInteractable) return;
        
        handRigidbody.gameObject.SetActive(false);
        configJoint.connectedBody = null;
        CancelInvoke(nameof(ShowHands));
    }

    private void SelectExited(SelectExitEventArgs arg0)
    {
        if (arg0.interactable is BaseTeleportationInteractable) return;

        Invoke(nameof(ShowHands),handShowDelay);
    }

    private void ShowHands()
    {
        handRigidbody.gameObject.SetActive(true);
        
        // Set the rb position and rotation as the Left/RightHandController values
        handRigidbody.transform.position = controller.transform.position;
        handRigidbody.transform.rotation =
                Quaternion.Euler(controller.transform.eulerAngles + _originalHandRot.eulerAngles);

        configJoint.connectedBody = handRigidbody;
    }
}
}



 