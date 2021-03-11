using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] GameObject door;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MovableBox") || other.CompareTag("SlimeChild") || other.CompareTag("Player"))
        {
            door.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        door.SetActive(true);
    }
}
