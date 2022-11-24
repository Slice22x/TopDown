using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Create Level")]
public class Level : ScriptableObject
{
    [Header("Identification")]
    public int World;
    public int LevelNumber;
    public string LevelName;
    public string LevelID;
    public int LevelIndex;

    [Space]
    [Header("Preformance")]
    public int BulletsShot;
    public int EnemiesCured;
    public int EnemiesDead;
    public int DamageTaken;
    public int Deaths;
    [Tooltip("This does not reset even when going into new levels")]
    int TotalDeaths;
    int TotalDamageTaken;
    int TotalEnemiesDead;
    int TotalEnemiesCured;
    int TotalBulletsShot;

    public static void AddToTotal(Level level)
    {
        level.TotalBulletsShot += level.BulletsShot;
        level.TotalDamageTaken += level.DamageTaken;
        level.TotalDeaths += level.Deaths;
        level.TotalEnemiesCured += level.EnemiesCured;
        level.TotalEnemiesDead += level.EnemiesDead;
    }

    public static void ResetLevelInfo(Level level, bool Death)
    {
        level.BulletsShot = 0;
        level.EnemiesCured = 0;
        level.EnemiesDead = 0;
        level.DamageTaken = 0;
        if(Death)
            level.Deaths = 0;
    }
}
