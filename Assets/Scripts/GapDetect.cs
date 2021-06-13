using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GapDetect : MonoBehaviour
{
    [SerializeField] GameObject gapObject;

    [SerializeField] BoxCollider bcBox;
    private void Start()
    {
        gapObject.SetActive(false);
    }

    private void FixedUpdate()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovableBox"))
        {
            gapObject.SetActive(true);
            bcBox.isTrigger = true;
        }
    }
}
