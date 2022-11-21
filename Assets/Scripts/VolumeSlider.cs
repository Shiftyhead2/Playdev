using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Loader loader;

    public Slider slider;

    public bool MusicSlider;

    void Awake()
    {
        loader = FindObjectOfType<Loader>();
        if (loader != null)
        {
            if (MusicSlider)
            {
                slider.value = loader.MusicVolume;
            }
            else
            {
                slider.value = loader.volume;
            }
        }
    }

    

    public void ChangeVolume(float amount)
    {
        if (loader != null)
        {
            loader.ChangeVolume(amount);
        }
    }

    public void ChangeMusicVolume(float amount)
    {
        if(loader != null)
        {
            loader.ChangeMusicVolume(amount);
        }
    }
}
