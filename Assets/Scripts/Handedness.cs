using UnityEngine;

public class Handedness : MonoBehaviour
{
    public enum Handed { Left, Right}

    public Handed handed;

    [SerializeField] private GameObject[] leftHandedObjects;
    [SerializeField] private GameObject[] rightHandedObjects;

    private void Awake()
    {
        if (handed == Handed.Left)
        {
            foreach (var obj in leftHandedObjects)
            {
                obj.SetActive(true);
            }

            foreach (var obj in rightHandedObjects)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            foreach (var obj in leftHandedObjects)
            {
                obj.SetActive(false);
            }

            foreach (var obj in rightHandedObjects)
            {
                obj.SetActive(true);
            }
        }
    }
}
