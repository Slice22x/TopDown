using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Create Item")]
public class ItemScriptableObject : ScriptableObject
{
    [Header("Information")]
    public string Name; 
    [TextArea(3, 10)]
    public string Description; 
    public int Level; 
    [Range(1,10)]
    public int Rarity; 
    public Sprite ArtWork; 
    public int Cost;
    public int SellValue;
    public string ID; 
    public GameObject ItemGameObject;
    public Color ItemColor;
    public ItemStyle[] Style;
    public TypeOfNPC.Personality[] AppealsTo = new TypeOfNPC.Personality[5];

    [Header("Modifications")]
    public int DamageAdder; 
    public int DefenceAdder;
    public int PenatrationAdder;
    public float ZoomAdder;
    public int PlayerSpeedAdder;
    public float BulletSpeedAdder;
    [Space]
    public bool Automate;
    public float MinSpreadMinus, MaxSpreadMinus;
    public float FireRateAdder;
    public float ReloadTimeMinus;
    [Space]
    public float DebufTimeAdder;
    public float DebufMultiplierAdder;
    public int ElementDamage;
    [Space]
    [Range(0.1f,1)]
    public float DamageRate;
    public int MaxAmmoAdder;
    public int HolerAmmoAdder;

    [Header("Debuffs")]
    public Enemies.Effect CauseEffect;

    public CraftingRecipies Recipie;

    public enum ItemStyle
    {
        Armour,
        Potion,
        Accecory,
        Element,
    }
}
