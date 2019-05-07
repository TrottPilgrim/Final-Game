using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BackgroundAudio : MonoBehaviour
{
    public static BackgroundAudio Instance;
    public AudioClip[] stings;
    public AudioSource stingSource;

    public AudioMixerSnapshot titleScreen;
    public AudioMixerSnapshot swapGame;
    
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(Instance);
        stingSource = transform.GetChild(0).GetComponent<AudioSource>();
    }

    public void PlaySound(string s)
    {
        AudioClip nextClip;
        switch(s)
        {
            case "click" :
                nextClip = stings[0];
                break;
            default:
                nextClip = stings[0];
                break;
        }
        stingSource.clip = nextClip;
        stingSource.Play();
    }
}
