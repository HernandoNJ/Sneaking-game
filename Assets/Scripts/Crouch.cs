using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    [SerializeField] private CharacterController _charController;
    [SerializeField] private float _crouchHeight = 1;
    
    private float _originalHeight;
    private bool _crouched = false;
    void Start()
    {
        _originalHeight = _charController.height;
    }
    
    void Update()
    {
        
    }

    private void OnCrouch()
    {
        if (_crouched)
        {
            _crouched = false;
            _charController.height = _originalHeight;
            Debug.Log("Player got up");
        }
        else
        {
            _crouched = true;
            _charController.height = _crouchHeight;
            Debug.Log("Player crouched down");
        }
    }
}
