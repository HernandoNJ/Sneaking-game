using UnityEngine;
using UnityEngine.Serialization;

public class DoorTrigger : MonoBehaviour
{
    [FormerlySerializedAs("door")]
    [SerializeField] private GameObject cube;
    [SerializeField] private float timeToStay;
    [SerializeField] private float currentTimeStayed;

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<DoorInteractor>())
        {
            currentTimeStayed += Time.deltaTime;

            if (currentTimeStayed > timeToStay)
            {
                cube.SetActive(true);
            }
        }
    }
}
