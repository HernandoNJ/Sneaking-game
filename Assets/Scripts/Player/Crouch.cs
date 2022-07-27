using UnityEngine;

public class Crouch : MonoBehaviour
{
    [SerializeField] private CharacterController charController;
    [SerializeField] private float crouchHeight = 1;
    
    private float _originalHeight;
    private bool _crouched = false;
    void Start()
    {
        _originalHeight = charController.height;
    }
    
    void Update()
    {
        
    }

    private void OnCrouch()
    {
        if (_crouched)
        {
            _crouched = false;
            charController.height = _originalHeight;
            Debug.Log("Player got up");
        }
        else
        {
            _crouched = true;
            charController.height = crouchHeight;
            Debug.Log("Player crouched down");
        }
    }
}
