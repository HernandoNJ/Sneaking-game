using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Hands
{
public class XRTeleportManager : MonoBehaviour
{
    [SerializeField] private XRBaseInteractor teleportController;
    [SerializeField] private XRBaseInteractor mainController;
    
    [SerializeField] private Animator handAnimator;
    [SerializeField] private GameObject pointer;

    private void Start()
    {
        teleportController.selectEntered.AddListener(MoveSelectionToMainController);
    }

    private void Update()
    {
        pointer.SetActive(handAnimator.GetCurrentAnimatorStateInfo(0).IsName("Point")&& handAnimator.gameObject.activeSelf);
    }

    private void MoveSelectionToMainController(SelectEnterEventArgs arg0)
    {
        var interactable = arg0.interactable;
        
        if(arg0.interactable is BaseTeleportationInteractable) return;
        
        if(interactable) teleportController.interactionManager.ForceSelect(mainController,interactable);
    }
}
}
