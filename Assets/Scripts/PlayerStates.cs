using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    Player player;
    MouseLook mouseLook;
    public enum PlayerMonsterStates
    {
        Big,
        Medium,
        Smol,
        Dead,
        InMenu,
        Cutscene,
        GrownMedium
    }

    public PlayerMonsterStates currentState;

    void Start()
    {
        player = GetComponent<Player>();
        mouseLook = GameObject.FindWithTag("MainCamera").GetComponent<MouseLook>();
    }

    void Update()
    {
        switch(currentState)
        {
            case PlayerMonsterStates.Big:
                player.Move();
                player.Big();
                player.Roar();
                mouseLook.CameraLook();
                Debug.Log("Big monster time");
                break;
            case PlayerMonsterStates.Medium:
                player.Move();
                player.Medium();
                player.CreateSlimeChild();
                mouseLook.CameraLook();
                Debug.Log("Medium monster time");
                break;
            case PlayerMonsterStates.Smol:
                player.Smol();
                player.Move();
                mouseLook.CameraLook();
                Debug.Log("Smol monster time");
                break;
            case PlayerMonsterStates.Dead:
                player.Dead();
                Debug.Log("u died");
                break;
            case PlayerMonsterStates.InMenu:
                Debug.Log("inmenu");
                Cursor.lockState = CursorLockMode.None;
                break;
            case PlayerMonsterStates.Cutscene:
                Debug.Log("cutscene now");
                break;
            case PlayerMonsterStates.GrownMedium:
                Debug.Log("Grown");
                player.Grow();
                player.Move();
                mouseLook.CameraLook();
                break;
        }
    }
}
