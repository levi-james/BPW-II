using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PressurePlate_Multiple : MonoBehaviour
{
    [SerializeField] AudioSource closed;
    [SerializeField] AudioSource open;
    [SerializeField] GameObject[] doors;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MovableBox") || other.CompareTag("SlimeChild") || other.CompareTag("Player") || other.CompareTag("LilFella"))
        {
            foreach (GameObject door in doors)
            {
                door.SetActive(false);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovableBox") || other.CompareTag("SlimeChild") || other.CompareTag("Player") || other.CompareTag("LilFella"))
        {
            open.Play();

        }
    }
    private void OnTriggerExit(Collider other)
    {

        foreach (GameObject door in doors)
        {
            door.SetActive(true);
            closed.Play();
        }
    }
}
