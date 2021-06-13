using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3 : MonoBehaviour
{
    PlayerStates states;
    // Start is called before the first frame update
    void Start()
    {
        states = GameObject.Find("Player").GetComponent<PlayerStates>();

        states.currentState = PlayerStates.PlayerMonsterStates.Medium;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
