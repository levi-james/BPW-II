using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
public class Player : MonoBehaviour
{
    PlayerStates states;

    //walking speeds
    [SerializeField] float smolSpeed;
    [SerializeField] float bigSpeed;
    [SerializeField] float medSpeed;
    [SerializeField] float speed;

    //grow size
    Vector3 growSize = new Vector3(2, 6, 2);
    //big monster push power
    [SerializeField] private float pushPower = 5f;

    //some references
    public CharacterController controller;
    public GameObject mainCam;
    public GameObject deadCam;

    Transform playerObj;
    public Animator smokeAnim;
    GameObject winCanvas;
    GameObject handsCanvas;


    //options menu
    GameObject menu;
    bool isInMenu = false;
    string previousState;

    //verschillende hands 
    [SerializeField] GameObject BigVision;
    [SerializeField] GameObject MediumVision;
    [SerializeField] GameObject SmolVision;

    [SerializeField] GameObject childPos;

    //slime ability
    GameObject slimeChild;
    private int slimeChildrenInScene = 0;

    //MONSTER AUDIO 
    [SerializeField] AudioSource chomp;
    [SerializeField] AudioSource roar;
    [SerializeField] AudioSource Small_footstep;
    [SerializeField] AudioSource Medium_footstep;
    [SerializeField] AudioSource Big_footstep;
    [SerializeField] AudioSource grow;
    [SerializeField] AudioSource pushBox;
    [SerializeField] AudioSource winSound;
    [SerializeField] AudioSource deadSound;
    bool isRoarPlaying;
    bool isPushSoundPlaying;
    bool isDeadSoundPlaying;

    //gravity idk
    Vector3 velocity;
    [SerializeField] float gravity = -9.81f;

    private void Start()
    {
        states = GetComponent<PlayerStates>();

        playerObj = GameObject.Find("Player").GetComponent<Transform>();
        slimeChild = GameObject.Find("SlimeChild");

        winCanvas = GameObject.Find("WinCanvas");
        menu = GameObject.Find("Menu");
        handsCanvas = GameObject.Find("HandsCanvas");

        winCanvas.SetActive(false);
        menu.SetActive(false);

    }
    void Update()
    {
        Restart();
        Menu();
       
        //DebugMonsters();
    }

    void Menu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isInMenu)
        {
            previousState = states.currentState.ToString();
            Debug.Log("state saved = " + previousState);
            ShowMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isInMenu)
        {
            HideMenu();
            states.currentState = (PlayerStates.PlayerMonsterStates)System.Enum.Parse(typeof(PlayerStates.PlayerMonsterStates), previousState);
        }

    }
    void ShowMenu()
    {
        Cursor.visible = true;
        menu.SetActive(true);
         states.currentState = PlayerStates.PlayerMonsterStates.InMenu;
         isInMenu = true;
    }

    void HideMenu()
    {
        Cursor.visible = false;
        menu.SetActive(false);
        isInMenu = false;
    }

    public void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        
    }
    /*---------------------------SLIME ABILITY---------------------------*/
    public void CreateSlimeChild()
    {

        if (slimeChildrenInScene < 1 && Input.GetMouseButtonDown(0))
        {
            Instantiate(slimeChild, childPos.transform.position, Quaternion.identity);
            slimeChildrenInScene++;
            Medium_footstep.Play();

            
            
        }

    }

    /*---------------------------POTION GROW ---------------------------*/
    public void Grow()
    {
        playerObj.transform.localScale = Vector3.Lerp(transform.localScale, growSize, 1 * Time.deltaTime);
    }


    /*---------------------------ROAR RAYCAST ABILITY---------------------------*/
    public void Roar()
    {
        int layer = 1 << 8;

        RaycastHit hit;

        if (roar.isPlaying)
        {
            isRoarPlaying = true;
        }
        else
        {
            isRoarPlaying = false;
        }

        if (Input.GetMouseButtonDown(0) && !isRoarPlaying)
        {
            roar.Play();
            roar.pitch = Random.Range(1, 2);
        }


        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layer))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            Debug.Log("Did Hit");

            
            

            if (Input.GetMouseButton(0))
            {
                Vector3 playerPos = transform.position;
                float distance = Vector3.Distance(hit.transform.position, playerPos);

                if (distance < 15)
                {
                    
                    Debug.Log("roar!");
                    

                    Vector3 dir = hit.transform.position - transform.position;
                    dir = dir.normalized;
                    hit.rigidbody.AddForce(dir * smolSpeed);

                    
                }
            }


        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }

        
    }

    /*---------------------------VERY MESSY EATING ABILITY---------------------------*/
    private void OnTriggerEnter(Collider other)
    {
        //finishing a level teleports you to the next one
        if(other.CompareTag("Finish"))
        {
            winSound.Play();
            states.currentState = PlayerStates.PlayerMonsterStates.Cutscene;
            winCanvas.SetActive(true);
            StartCoroutine(GoToNextLevel());
        }

        if (other.CompareTag("Potion") && states.currentState == PlayerStates.PlayerMonsterStates.Medium)
        {
            grow.Play();
            states.currentState = PlayerStates.PlayerMonsterStates.GrownMedium;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("LilFella"))
        {
            chomp.Play();
            Destroy(other.gameObject);
            states.currentState = PlayerStates.PlayerMonsterStates.Smol;

        }

        if (other.CompareTag("MediumFella") && states.currentState == PlayerStates.PlayerMonsterStates.Big)
        {
            chomp.Play();

            Destroy(other.gameObject);
            states.currentState = PlayerStates.PlayerMonsterStates.Medium;
        }

        if (other.CompareTag("BigFella") && states.currentState == PlayerStates.PlayerMonsterStates.Medium)
        {
            states.currentState = PlayerStates.PlayerMonsterStates.Dead;
        }

        if (other.CompareTag("BigFella") && states.currentState == PlayerStates.PlayerMonsterStates.Big)
        {
            states.currentState = PlayerStates.PlayerMonsterStates.Dead;
        }

        if (other.CompareTag("BigFella") && states.currentState == PlayerStates.PlayerMonsterStates.Smol)
        {
            states.currentState = PlayerStates.PlayerMonsterStates.Dead;
        }

        if (other.CompareTag("BigFella") && states.currentState == PlayerStates.PlayerMonsterStates.GrownMedium)
        {
            chomp.Play();
            states.currentState = PlayerStates.PlayerMonsterStates.Big;
            Destroy(other.gameObject);
            
        }


        if (other.CompareTag("EatCollider") && states.currentState == PlayerStates.PlayerMonsterStates.Big)
        {
            chomp.Play();
            Destroy(other.gameObject);
            states.currentState = PlayerStates.PlayerMonsterStates.Smol;
        }
    }



    /*---------------------------PUSHING ABILITY---------------------------*/
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.collider.CompareTag("MovableBox") && states.currentState == PlayerStates.PlayerMonsterStates.Big)
        {
            Rigidbody body = hit.collider.attachedRigidbody;

            // no rigidbody
            if (body == null || body.isKinematic)
            {
                pushBox.Stop();
                return;
            }

            // We dont want to push objects below us
            if (hit.moveDirection.y < -0.3)
            {
                pushBox.Stop();
                return;
            }

            // Calculate push direction from move direction,
            // we only push objects to the sides never up and down
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            // If you know how fast your character is trying to move,
            // then you can also multiply the push velocity by that.
            
            
            if (pushBox.isPlaying)
            {
                isPushSoundPlaying= true;
            }
            else
            {
                isPushSoundPlaying = false;
            }

            if (!isPushSoundPlaying)
            {
                pushBox.Play();
            }
            // Apply the push
            body.velocity = pushDir * pushPower;
        }
        else if(hit.collider.CompareTag("SlimeChild") && states.currentState == PlayerStates.PlayerMonsterStates.Medium)
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
    }


    /*---------------------------STATE FUNCTIONS---------------------------*/
    public void Smol()
    {
        playerObj.transform.localScale = new Vector3(1, 0.5f, 1);
       
        SmolVision.SetActive(true);
        MediumVision.SetActive(false);
        BigVision.SetActive(false);
        speed = smolSpeed;

    }

    public void Medium()
    {
        
        playerObj.transform.localScale = new Vector3(1.5f, 3, 1.5f);
        
        SmolVision.SetActive(false);
        MediumVision.SetActive(true);
        BigVision.SetActive(false);
        speed = medSpeed;
    }

    public void Big()
    {
        playerObj.transform.localScale = new Vector3(2, 4, 2);

        SmolVision.SetActive(false);
        MediumVision.SetActive(false);
        BigVision.SetActive(true);
        speed = bigSpeed;

    }

    public void Dead()
    {
        
        mainCam.SetActive(false);
        smokeAnim.SetBool("isDead", true);
        if (deadSound.isPlaying)
        {
            isDeadSoundPlaying = true;
        }

        if(!isDeadSoundPlaying)
        {
            deadSound.Play();
        }

        
        deadCam.SetActive(true);
        handsCanvas.SetActive(false);
        
        GetComponent<MeshRenderer>().enabled = false;
        

    }
    /*---------------------------OVERIGE FUNCTIONS---------------------------*/
    public IEnumerator GoToNextLevel()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }


    public void Restart()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
    }

    void DebugMonsters()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            states.currentState = PlayerStates.PlayerMonsterStates.Smol;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            states.currentState = PlayerStates.PlayerMonsterStates.Medium;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            states.currentState = PlayerStates.PlayerMonsterStates.Big;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            states.currentState = PlayerStates.PlayerMonsterStates.GrownMedium;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            states.currentState = PlayerStates.PlayerMonsterStates.Dead;
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            StartCoroutine(GoToNextLevel());
        }

    }
}
