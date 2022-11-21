using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audio;
    public static SoundManager instance;

    [SerializeField]
    private AudioClip cashAudio;
    [SerializeField]
    private AudioClip clickAudio;
    [SerializeField]
    private AudioClip eventAudio;
    

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        audio = this.GetComponent<AudioSource>();
    }

   

    public void PlayClickAudio()
    {
        audio.Stop();
        audio.clip = clickAudio;
        audio.Play();
    }

    
}
