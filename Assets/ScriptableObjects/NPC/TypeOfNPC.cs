using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New NPC", menuName = "Create NPC")]
public class TypeOfNPC : ScriptableObject
{
    public enum Personality
    {
        Null,
        Architect,
        Logician,
        Commander,
        Debater,
        Advocate,
        Mediator,
        Protagonist,
        Campainger,
        Logistian,
        Defender,
        Executive,
        Consul,
        Virtuoso,
        Adventurer,
        Entrepreneur,
        Entertainer,
        Introvert,
        Extrovert,
        Bossy,
        Hateful,
        Entiled,
        Humble,
        Quirky,
        KAREN
    }

    public enum Roles
    {
        Villager,
        Mayor,
        Defence,
        Law,
        Worker,
        Pope,
        Outcast,
    }

    [Header("About The Character")]
    public string Name;
    public Personality[] Personalities;
    public float Speed;
    [Range(0.1f, 1f)]
    public float TalkSpeed;
    public Roles Role;
    public HouseInfo House;

    public int LoveForPlayer;

    public int LoveForItem(Item ItemToTest)
    {
        int LoveForPlayer = new int();
        if(ItemToTest.ItemOfChoice.ItemColor == FavouriteColour)
        {
            LoveForPlayer += 10;
        }

        foreach (Personality Person in Personalities)
        {
            foreach (Personality ItemPerson in ItemToTest.ItemOfChoice.AppealsTo)
            {
                if(ItemPerson == Person)
                {
                    LoveForPlayer += 2;
                }
            }
        }
        if(ItemToTest.ItemOfChoice == FavItem)
        {
            LoveForPlayer += 15;
        }
        if (ItemToTest.ItemOfChoice == QuestItem)
        {
            LoveForPlayer += 10;
        }
        return LoveForPlayer;
    }
    [Header("Favourites")]
    public Color FavouriteColour;
    public ItemScriptableObject FavItem;
    public ItemScriptableObject QuestItem;
}
