using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

namespace Weapons
{
public class ShockPistol : Gun
{
    [SerializeField] private Renderer _gunRenderer;
    [SerializeField] private Material[] _ammoScreenMaterials;

    protected override void Start()
    {
        base.Start();
        Assert.IsNotNull(_gunRenderer, "You have not assigned a renderer to gun " + name);
        Assert.IsNotNull(_ammoScreenMaterials, "You have not ammo materials to gun " + name);
    }

    protected override void AmmoAttached(SelectEnterEventArgs arg0)
    {
        base.AmmoAttached(arg0);
        UpdateShockPistolScreen();
    }

    protected override void AmmoDetached(SelectExitEventArgs arg0)
    {
        base.AmmoDetached(arg0);
        UpdateShockPistolScreen();
    }

    protected override void Fire(ActivateEventArgs arg0)
    {
        if (!CanFire()) return;

        base.Fire(arg0);
        UpdateShockPistolScreen();

        var bullet = Instantiate(_ammoClip.bulletObject, gunBarrel.position, quaternion.identity)
                .GetComponent<Rigidbody>();
        bullet.AddForce(gunBarrel.forward * _ammoClip.bulletSpeed, ForceMode.Impulse);
        Destroy(bullet.gameObject, 6f);
    }

    private void UpdateShockPistolScreen()
    {
        if (!_ammoClip)
        {
            AssignedScreenMaterial(_ammoScreenMaterials[0]);

            return;
        }

        AssignedScreenMaterial(_ammoScreenMaterials[_ammoClip.amount]);
    }

    private void AssignedScreenMaterial(Material newMaterial)
    {
        var mats = _gunRenderer.materials;
        mats[1] = newMaterial;
        _gunRenderer.materials = mats;
    }
}
}
