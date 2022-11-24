using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public List<Sound> Sounds;

    public SettingsObj Settings;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(gameObject);

        foreach (Sound s in Sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;

            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.Loop;
        }
    }

    private void Update()
    {
        foreach (Sound s in Instance.Sounds)
        {
            switch (s.ThisSound)
            {
                case Sound.SoundType.Music:
                    s.Source.volume = (s.Volume * Settings.MusicVolume) * Settings.MasterVolume;
                    break;
                case Sound.SoundType.SFX:
                    s.Source.volume = (s.Volume * Settings.SFXVolume) * Settings.MasterVolume;
                    break;
            }
        }
    }

    public void SetMaVolume(Slider slider)
    {
        slider.value = Settings.MasterVolume;
    }

    public void SetSfxVolume(Slider slider)
    {
        slider.value = Settings.SFXVolume;
    }

    public void SetMuVolume(Slider slider)
    {
        slider.value = Settings.MusicVolume;
    }

    public static void StopAll()
    {
        foreach (Sound s in Instance.Sounds)
        {
            s.Source.Stop();
        }
    }

    public static void Stop(string Name)
    {
        Sound S = Instance.Sounds.Find(s => s.Name == Name);
        S.Source.Stop();
    }

    public static void Play(string Name)
    {
        Sound S = Instance.Sounds.Find(s => s.Name == Name);
        S.Source.Play();
    }
}
