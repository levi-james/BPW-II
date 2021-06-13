using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] AudioSource open;
    [SerializeField] AudioSource closed;
    [SerializeField] GameObject door;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MovableBox") || other.CompareTag("SlimeChild") || other.CompareTag("Player") || other.CompareTag("LilFella"))
        {
            door.SetActive(false);
            
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
        closed.Play();
        door.SetActive(true);
        
    }
}
