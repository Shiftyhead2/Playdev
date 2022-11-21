using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData 
{
    public float GameVolume;
    public float musicVolume;

    public SettingsData(Loader loader)
    {
        GameVolume = loader.volume;
        musicVolume = loader.MusicVolume;
    }
}
