using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorKeyCardTrigger : DoorTrigger
{
    [SerializeField] private int keyCardLevel = 1;
    [SerializeField] private Renderer lightObject;
    [SerializeField] private Light socketLight;
    [SerializeField] private XRSocketInteractor socket;
    [SerializeField] private Color defaultColor = Color.yellow;
    [SerializeField] private Color errorColor = Color.red;
    [SerializeField] private Color successColor = Color.green;

    private bool _isOpen;

    private void Start()
    {
        SetLightColor(defaultColor);

        socket.selectEntered.AddListener(KeyCardInserted);
        socket.selectExited.AddListener(KeyCardRemoved);
    }

    private void KeyCardInserted(SelectEnterEventArgs arg0)
    {
        if (!arg0.interactable.TryGetComponent(out KeyCard keyCard))
        {
            Debug.LogWarning("Inserted object has no keyCard");
            SetLightColor(errorColor);
            return;
        }

        if (keyCard.keycardLevel >= keyCardLevel)
        {
            SetLightColor(successColor);
            _isOpen = true;
            OpenDoor();
        }
        else SetLightColor(errorColor);
    }

    private void KeyCardRemoved(SelectExitEventArgs arg0)
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
