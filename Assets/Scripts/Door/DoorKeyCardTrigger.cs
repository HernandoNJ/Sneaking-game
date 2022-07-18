using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorKeyCardTrigger : DoorTrigger
{
    [SerializeField] private XRSocketInteractor socket;
    [SerializeField] private Renderer lightObject;
    [SerializeField] private Light socketLight;
    [SerializeField] private Color defaultColor = Color.yellow;
    [SerializeField] private Color errorColor = Color.red;
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private int keycardLevel = 1;
    
    
    private bool _isOpen = false;

    private void Start()
    {
        SetLightColor(defaultColor);
        
        socket.selectEntered.AddListener(KeycardInserted);
        socket.selectExited.AddListener(KeycardRemoved);
    }

    private void KeycardInserted(SelectEnterEventArgs arg0)
    {
        if (!arg0.interactable.TryGetComponent(out KeyCard keyCard))
        {
            Debug.LogWarning("Inserted object has no keycard");
            SetLightColor(errorColor);
            return;
        }

        if (keyCard.keycardLevel >= keycardLevel)
        {
            SetLightColor(successColor);
            _isOpen = true;
            OpenDoor();
        }
        else
        {
            SetLightColor(errorColor);
        }
    }

    private void KeycardRemoved(SelectExitEventArgs arg0)
    {
        SetLightColor(defaultColor);
        _isOpen = false;
        ClosedDoor();
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (_isOpen) return;
        
        base.OnTriggerExit(other);
    }

    private void SetLightColor(Color color)
    {
        lightObject.material.color = color;
        socketLight.color = color;
    }

}
