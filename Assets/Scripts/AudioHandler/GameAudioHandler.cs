using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioHandler : MonoBehaviour
{
    public static GameAudioHandler instance;

    public AudioSource coinAudioSource;
    public AudioSource deathAudioSource;
    public AudioSource throwAudioSource;
    public AudioSource buttonAudioSource;

    void Start()
    {
        if(instance!=null)
        {
            return;
        }
        else
        {
            instance=this;
        }
    }

    public void PlayThrowSound()
    {
        if(GameController.instance.sfxIndex==0)
        {
            throwAudioSource.Play();
        }
    }

    public void PlayButtonClickSound()
    {
        if(GameController.instance.sfxIndex==0)
        {
            buttonAudioSource.Play();
        }
    }

    public void PlayDeathSound()
    {
        if(GameController.instance.sfxIndex==0)
        {
            deathAudioSource.Play();
        }
    }

    public void PlayCoinSound()
    {
        if(GameController.instance.sfxIndex==0)
        {
            coinAudioSource.Play();
        }
    }
}
