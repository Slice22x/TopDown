using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundSlider : MonoBehaviour
{
    public bool Music, Master, SFX;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text SliderNumber;
    void Start()
    {
        if (Music)
        {
            SoundManager.Instance.SetMuVolume(slider);
        }
        else if (Master)
        {
            SoundManager.Instance.SetMaVolume(slider);
        }
        else if (SFX)
        {
            SoundManager.Instance.SetSfxVolume(slider);

        }
    }

    // Update is called once per frame
    void Update()
    {
        SliderNumber.text = Mathf.RoundToInt(slider.value * 100).ToString();
    }

    public void ChangeMaVolume(Slider slider)
    {
        SoundManager.Instance.Settings.MasterVolume = slider.value;
    }

    public void ChangeSfxVolume(Slider slider)
    {
        SoundManager.Instance.Settings.SFXVolume = slider.value;
    }

    public void ChangeMuVolume(Slider slider)
    {
        SoundManager.Instance.Settings.MusicVolume = slider.value;
    }
}
