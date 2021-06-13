using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    public AudioMixer audioMixer;
    //h ttps://www.youtube.com/watch?v=YOaYQrN1oYQ
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
