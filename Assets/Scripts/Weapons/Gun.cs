using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

namespace Weapons
{
public class Gun : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable grabInteractable;
    [SerializeField] protected Transform gunBarrel;
    [SerializeField] private XRSocketInteractor ammoSocket;

    protected AmmoClip _ammoClip;
    
    protected virtual void Start()
    {
        Assert.IsNotNull(grabInteractable, "You have not assigned grab interactable to gun " + name);
        Assert.IsNotNull(gunBarrel, "You have not assigned gunBarrel to gun " + name);
        Assert.IsNotNull(ammoSocket, "You have not assigned ammoSocket to gun " + name);

        ammoSocket.selectEntered.AddListener(AmmoAttached);
        ammoSocket.selectExited.AddListener(AmmoDetached);

        grabInteractable.activated.AddListener(Fire);
    }

    protected virtual void AmmoAttached(SelectEnterEventArgs arg0)
    {
        IgnoreCollision(arg0.interactable,true);
        _ammoClip = arg0.interactable.GetComponent<AmmoClip>();
    }

    protected virtual void AmmoDetached(SelectExitEventArgs arg0)
    {
        IgnoreCollision(arg0.interactable,false);
        _ammoClip = null;
    }

    protected virtual void Fire(ActivateEventArgs arg0)
    {
        if (!CanFire()) return;
        _ammoClip.amount -= 1;
    }

    protected virtual bool CanFire()
    {
        if (!_ammoClip)
        {
            Debug.Log("No ammo clip inserted");
            return false;
        }
        if(_ammoClip.amount <= 0)
        {
            _ammoClip.amount = 0;
            Debug.Log("No ammo left");
            return false;
        }

        return true;
    }

    private void IgnoreCollision(XRBaseInteractable interactable, bool ignore)
    {
        var myColliders = GetComponentsInChildren<Collider>();

        foreach (var myCollider in myColliders)
        {
            foreach (var interactableCollider in interactable.colliders)
            {
                Physics.IgnoreCollision(myCollider,interactableCollider,ignore);
            }
        }
    }

}
}
