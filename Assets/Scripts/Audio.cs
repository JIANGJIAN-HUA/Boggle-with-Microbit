using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource BGMusic;
    public AudioSource ShakeDicesMusic;
    public AudioSource GameFinishMusic;

    public void PlayBG()
    {
        BGMusic.Play();
    }
    public void PauseBG()
    {
        BGMusic.Pause();
    }
    public void UnPauseBG()
    {
        BGMusic.UnPause();
    }
    public void StopBG()
    {
        BGMusic.Stop();
    }

    public void PlaySD()
    {
        ShakeDicesMusic.Play();
    }
    public void StopSD()
    {
        ShakeDicesMusic.Stop();
    }

    public void PlayGF()
    {
        GameFinishMusic.Play();
    }
}
