using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Audio;

public class Loader : MonoBehaviour
{
    private static Loader instance;
    public bool GameIsLoaded;
    

    public float volume = 1f;
    public float MusicVolume = 1f;

    public AudioMixer SoundVolume;
    public AudioMixer MusicMixer;


    void Awake()
    {
        DontDestroyOnLoad(this);

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        string path = Application.persistentDataPath + "/Config.data";

        if (File.Exists(path))
        {
            LoadData();
        }
        else
        {


        }

    }

    void Start()
    {
        SoundVolume.SetFloat("SoundVolume", Mathf.Log10(volume)*20);
        MusicMixer.SetFloat("MusicVolume", Mathf.Log10(MusicVolume) * 20);
    }
    
    

    public void ChangeVolume(float vol)
    {
        volume = vol;
        SoundVolume.SetFloat("SoundVolume", Mathf.Log10(volume) * 20);
    }

   public void ChangeMusicVolume(float current)
    {
        MusicVolume = current;
        MusicMixer.SetFloat("MusicVolume", Mathf.Log10(MusicVolume) * 20);
    }

    void LoadData()
    {
        //string path = Application.persistentDataPath + "/Config.data";
        //Debug.Log("Loading Data found at " + path);
        SettingsData data = SaveSystem.LoadSettings();
        volume = data.GameVolume;
        MusicVolume = data.musicVolume;

        //Debug.Log("Sound volume: " + volume);
        //Debug.Log("Music volume: " + MusicVolume);
    }

    public void SaveData()
    {
        SaveSystem.SaveSettings(this);
    }
}
