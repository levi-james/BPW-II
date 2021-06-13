using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Flag : MonoBehaviour
{
    PlayerStates states;
    [SerializeField] GameObject winCanvas;
    [SerializeField] AudioSource winSound;

    private void Start()
    {
        states = GameObject.Find("Player").GetComponent<PlayerStates>();
        

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("LilFella"))
        {
            winSound.Play();
            states.currentState = PlayerStates.PlayerMonsterStates.Cutscene;
            winCanvas.SetActive(true);
            StartCoroutine(GoToNextLevel());
        }
    }

    public IEnumerator GoToNextLevel()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
