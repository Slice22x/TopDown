using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Create Dialogue")]
public class TypeOfDialogue : ScriptableObject
{
    public TypeOfNPC.Personality Personality;
    public bool SpeakingToObj;

    [System.Serializable]
    public struct Dialogue
    {
        public string Name;
        public enum TypeOfSpeach
        {
            Normal,
            LookAt,
            CallOver,
            Destroy,
            SpawnEnemy,
            Spawn,
        }

        public TypeOfSpeach Type;
        public string ActionObject;
        [TextArea(4, 15)]
        public string Speak;
    }

    [System.Serializable]
    public struct Speaches
    {
        public List<Dialogue> DialogueToSay;
    }

    public Speaches Speach;
}
