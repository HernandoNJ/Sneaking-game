using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject youWin;
    [SerializeField] private ParticleSystem youWinParticles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DoorInteractor>())
        {
            door.SetActive(false);

            if (other.CompareTag("Player"))
            {
                Debug.Log("Player: you've reached the goal");
                EnableWinUI(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<DoorInteractor>())
        {
            door.SetActive(true);
            EnableWinUI(false);
        }
    }

    private void EnableWinUI(bool winUIEnabled)
    {
        youWin.SetActive(winUIEnabled);
        if(winUIEnabled) youWinParticles.Play();
        else youWinParticles.Stop();
    }
}
