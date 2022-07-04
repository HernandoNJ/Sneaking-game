using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DoorInteractor>())
        {
            door.SetActive(false);

            if (other.CompareTag("Player"))
            {
                Debug.Log("Player: you've reached the goal");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<DoorInteractor>())
        {
            door.SetActive(true);
        }
    }
}
