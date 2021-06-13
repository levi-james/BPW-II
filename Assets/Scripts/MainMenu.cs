using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        
    }

    private void Update()
    {
        Scene sceneLoaded = SceneManager.GetActiveScene();

        if(sceneLoaded.name == "End")
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        Quit();


    }
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
