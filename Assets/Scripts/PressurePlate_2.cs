using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate_2 : MonoBehaviour
{
    int activePlates = 0;
    [SerializeField] GameObject door;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovableBox") || other.CompareTag("SlimeChild") || other.CompareTag("Player") || other.CompareTag("LilFella"))
        {

            activePlates++;
        }
    }

    private void Update()
    {
        if(activePlates >= 1)
        {
            door.SetActive(false);
        }
        else
        {
            door.SetActive(true);
        }

       

        Debug.Log(activePlates);
    }
    private void OnTriggerExit(Collider other)
    {
        door.SetActive(true);
        activePlates--;
    }
}
