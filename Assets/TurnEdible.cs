using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEdible : MonoBehaviour
{
    [SerializeField] BoxCollider LilFella;
    [SerializeField] Rigidbody RB;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("LilFella"))
        {
            LilFella.isTrigger = true;
            RB.isKinematic = true;
            
        }
    }
}
