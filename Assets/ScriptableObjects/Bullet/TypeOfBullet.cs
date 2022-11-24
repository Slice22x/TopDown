using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet", menuName = "Create Bullet")]
public class TypeOfBullet : ScriptableObject
{
    public int BaseDamage;
    public int BasePenatration;
    public float BaseRange;
    public float BaseSpeed;
    public bool CanPenetrate;
    [Range(0, 1)]
    public float CriticalChance;
    [Range(2,5)]
    public int CriticalMultiplier;


    public Sprite Artwork;
}
