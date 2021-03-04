using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    Player player;
    public enum PlayerMonsterStates
    {
        Big,
        Medium,
        Smol,
        Dead,
        Cutscene
    }

    public PlayerMonsterStates currentState;

    void Start()
    {
        player = GetComponent<Player>();
        //currentState = PlayerMonsterStates.Medium;
    }

    void Update()
    {
        switch(currentState)
        {
            case PlayerMonsterStates.Big:
                player.Move();
                player.Big();
                Debug.Log("Big monster time");
                break;
            case PlayerMonsterStates.Medium:
                player.Move();
                player.Medium();
                player.CreateSlimeChild();
                Debug.Log("Medium monster time");
                break;
            case PlayerMonsterStates.Smol:
                player.Smol();
                player.Move();
                Debug.Log("Smol monster time");
                break;
            case PlayerMonsterStates.Dead:
                Debug.Log("u died");
                break;
            case PlayerMonsterStates.Cutscene:
                Debug.Log("cutscene now");
                break;
        }
    }
}
