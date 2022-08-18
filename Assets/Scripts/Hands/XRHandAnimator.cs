using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRHandAnimator : MonoBehaviour
{
    [SerializeField] private ActionBasedController _controller;
    [SerializeField] private Animator _animator;
    
    private void Start()
    {
        _controller.activateAction.action.started += Fist;
        _controller.activateAction.action.canceled += FistReleased;
        
        _controller.selectAction.action.started += Point;
        _controller.selectAction.action.canceled += PointReleased;
    }

    private void OnDestroy()
    {
        _controller.activateAction.action.started -= Fist;
        _controller.activateAction.action.canceled -= FistReleased;
        
        _controller.selectAction.action.started -= Point;
        _controller.selectAction.action.canceled -= PointReleased;
    }

    private void Fist(InputAction.CallbackContext ctx)
    {
        _animator.SetBool("Fist", true);
    }

    private void FistReleased(InputAction.CallbackContext ctx)
    {
        _animator.SetBool("Fist",false);
    }

    private void Point(InputAction.CallbackContext ctx)
    {
        _animator.SetBool("Point",true);
    }

    private void PointReleased(InputAction.CallbackContext obj)
    {
        _animator.SetBool("Point",false);
    }
}
