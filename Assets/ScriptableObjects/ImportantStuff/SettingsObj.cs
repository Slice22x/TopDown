using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Settings", menuName = "Create Settings")]
public class SettingsObj : ScriptableObject
{
    public float MasterVolume;
    public float MusicVolume;
    public float SFXVolume;
}
