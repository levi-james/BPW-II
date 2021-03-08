using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    PlayerStates states;
    [SerializeField] float smolSpeed;
    [SerializeField] float bigSpeed;
    [SerializeField] float medSpeed;
    [SerializeField] float speed;

    [SerializeField] private float pushPower = 5f;

    public CharacterController controller;

    Transform playerObj;

    [SerializeField] GameObject BigVision;
    [SerializeField] GameObject MediumVision;
    [SerializeField] GameObject SmolVision;

    [SerializeField] GameObject childPos;
    GameObject slimeChild;
    private int slimeChildrenInScene = 0;

    [SerializeField] AudioSource chomp;

    void Update()
    {
        states = GetComponent<PlayerStates>();
        playerObj = GameObject.Find("Player").GetComponent<Transform>();
        slimeChild = GameObject.Find("SlimeChild");

    }

    public void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
    }

    public void CreateSlimeChild()
    {
        if (Input.GetKeyDown(KeyCode.X) && slimeChildrenInScene < 2)
        {
            Instantiate(slimeChild, childPos.transform.position, Quaternion.identity);
            slimeChildrenInScene++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //finishing a level teleports you to the next
        if(other.CompareTag("Finish"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }



        if (other.CompareTag("LilFella"))
        {
            chomp.Play();
            Destroy(other.gameObject);
            states.currentState = PlayerStates.PlayerMonsterStates.Smol;

        }

        if (other.CompareTag("MediumFella") && states.currentState == PlayerStates.PlayerMonsterStates.Big)
        {
            Destroy(other.gameObject);
            states.currentState = PlayerStates.PlayerMonsterStates.Medium;
        }

        if (other.CompareTag("BigFella") && states.currentState == PlayerStates.PlayerMonsterStates.Medium)
        {
            states.currentState = PlayerStates.PlayerMonsterStates.Big;
            Destroy(other.gameObject);
        }
    }

    //ht tps://docs.unity3d.com/ScriptReference/CharacterController.OnControllerColliderHit.html?_ga=2.251409286.1855934843.1615121525-522815912.1603372675
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;

    }

    public void Smol()
    {
        playerObj.transform.localScale = new Vector3(1, 0.2f, 1);
        SmolVision.SetActive(true);
        MediumVision.SetActive(false);
        BigVision.SetActive(false);
        speed = smolSpeed;

    }

    public void Medium()
    {
        playerObj.transform.localScale = new Vector3(1, 1f, 1);
        SmolVision.SetActive(false);
        MediumVision.SetActive(true);
        BigVision.SetActive(false);
        speed = medSpeed;
    }

    public void Big()
    {
        playerObj.transform.localScale = new Vector3(1, 1.5f, 1);
        SmolVision.SetActive(false);
        MediumVision.SetActive(false);
        BigVision.SetActive(true);
        speed = bigSpeed;

    }


    public void DebugMonsters()
    {
        
    }
}
