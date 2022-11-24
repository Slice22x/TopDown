using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Create Enemy")]
public class TypeOfEnemy : ScriptableObject
{
    public string Name;
    [TextArea(3, 10)]
    public string Description;
    public string ID;
    public Color EnemyColour;
    public Sprite EnemySprite;

    public int Health;
    [Range(0,100)]
    public int Defence;
    public int Damage;
    public int Speed;
    public float DetectionRange;
    public int WanderSpeed;
    [Range(0,100)]
    public float ElementResistance;

    public GameObject EnemyObjectt;

    public enum EnemyStyle
    {
        Esparagus,
        Eggplant,
        Starberry,
        Banana,
        RockCandy,
        CandyCane,
        Smarties,
        Lollipop,
        Potato,
        Spaghetti,
        Bread,
        Pizza
    }

    public EnemyStyle EnemyTpye;
}
