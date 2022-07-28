using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject door; // T Door

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DoorInteractor>())
        {
            OpenDoor();
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<DoorInteractor>())
        {
            ClosedDoor();
        }
    }

    protected void OpenDoor()
    {
        door.SetActive(false);
    }

    protected void ClosedDoor()
    {
        door.SetActive(true);
    }
}
