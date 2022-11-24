using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Create Weapon")]
public class TypeOfWeapon : ScriptableObject
{
    [Header("Info")]
    public string Name;
    [TextArea(3,10)]
    public string Description;
    [Range(1,9)]
    public int Level;
    public int Rarity;
    public Sprite Artwork;
    public enum Style
    {
        Rifle = 1,
        Sniper = 2,
        Pistol = 3,
        Melee = 4,
        LMG = 5,
        Shotgun = 6,
        MachineGun = 7,
        RocketLauncher = 8,
        Laser = 9,
        MultiBullet = 10,
        Charge = 11
    }
    public Style GunStyle;

    public enum RequiredAmmo
    {
        AKBullet,
        SniperBullet,
        MiniBullet,
        ShotgunBullet,
        MachineBullet,
        Rockets,
        Laser,
        SpecialBullets,
        RocketFuel
    }
    public RequiredAmmo Required;

    [Header("Bullet Mods")]
    public int ExtraDamage;
    public int ExtraPenatration;
    public float ExtraRange;
    public float ExtraSpeed;

    [Header("Weapon Action")]
    public TypeOfBullet Bull;
    public bool Automatic;
    public float MinSpread,MaxSpread;
    public float Zoom;
    [Range(0.1f, 100f)]
    public float FireRate;

    [Header("Cheats")]
    public bool UseAmmo = true;
    
    [Header("Ammo")]
    public float RealoadTime;
    public int Rounds;
    public int Ammo;

    [Header("Weapon Specific")]
    public int EveryNShots;
    public bool RangeNShots;
    public int2 EveryNShotsRange;
    [Space]
    public float StartAngle, EndAngle;
    public float Stableiser;
    [Space]
    [Range(0f, 100f)]
    public float ChargeMax;
    [Range(0.1f, 100f)]
    public float ChargeRate;
    public int ShotgunTurrets;
}
