using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XR
{
public class SmoothTeleportationAnchor : BaseTeleportationInteractable
{
    [SerializeField] private float _teleportSpeed = 3f;
    [SerializeField] private float _stoppingDistance = 0.1f;
    
    private Vector3 _teleportEnd;
    private bool _isTeleporting;
    private XRRig _rig;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log("OnSelectEntered");

        // To get the XR Rig from the interactor - GetComponentInParent
        BeginTeleport(args.interactor);
    }

    private void BeginTeleport(XRBaseInteractor interactor)
    {
        _rig = interactor.GetComponentInParent<XRRig>();
        
        // The interactor (Hand Controller) position
        // Is relative to the XR Rig position (has some offset)
        var interactorPos = interactor.transform.localPosition;
        interactorPos.y = 0f;
        
        // Subtract the interactor position to get the correct value
        _teleportEnd = transform.position - interactorPos;
        _isTeleporting = true;
    }

    private void Update()
    {
        if (_isTeleporting)
        {
            _rig.transform.position =
                    Vector3.MoveTowards(_rig.transform.position, _teleportEnd, _teleportSpeed * Time.deltaTime);

            if (Vector3.Distance(_rig.transform.position, _teleportEnd) < _stoppingDistance)
            {
                _isTeleporting = false;
            }
        }
    }
}
}
