using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    PlayerStates states;
    public float smolSpeed;
    public float bigSpeed;
    public float medSpeed; 
    public float speed = 12f;

    public CharacterController controller;

    Transform playerObj;

    public GameObject BigVision;
    public GameObject MediumVision;
    public GameObject SmolVision;

    [SerializeField] GameObject childPos;
    GameObject slimeChild;
    bool slimeChildCreated = false;
    int slimeChildrenInScene = 0;

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
        if(Input.GetKeyDown(KeyCode.X) && !slimeChildCreated)
        {
            Instantiate(slimeChild, childPos.transform.position, Quaternion.identity);
            slimeChildCreated = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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

        if (other.CompareTag("MediumFella"))
        {
            Destroy(other.gameObject);
            states.currentState = PlayerStates.PlayerMonsterStates.Medium;
        }

        if (other.CompareTag("BigFella"))
        {
            states.currentState = PlayerStates.PlayerMonsterStates.Big;
        }


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
