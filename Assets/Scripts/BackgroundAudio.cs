using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BackgroundAudio : MonoBehaviour
{
    public static BackgroundAudio Instance;
    [System.Serializable]
    public struct NamedAudio {
        public string name;
        public AudioClip[] clip;
    }

    public NamedAudio[] stings;
    // public AudioClip[] stings;
    public Dictionary<string, AudioClip[]> stingsDict = new Dictionary<string, AudioClip[]>();
    public AudioSource stingSource;

    public AudioMixerSnapshot titleScreen;
    public AudioMixerSnapshot swapGame;
    public AudioMixerSnapshot nearingEnd;
    
    void Start()
    {
        Instance = this;
        foreach (NamedAudio s in stings)
        {
            stingsDict.Add(s.name, s.clip);
        }

        DontDestroyOnLoad(Instance);
        stingSource = transform.GetChild(0).GetComponent<AudioSource>();
    }

    public void PlaySound(string s)
    {
        stingSource.clip = stingsDict[s][Random.Range(0, stingsDict[s].Length)];
        stingSource.Play();
    }
}
