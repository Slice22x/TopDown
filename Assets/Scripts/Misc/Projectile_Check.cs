using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile_Check : MonoBehaviour
{
    public static Projectile_Check Instance;
    public Projectile[] SpawnedProj;
    public bool DontSpawn;

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnedProj = FindObjectsOfType<Projectile>();
    }

    public void DestroyProjs(bool IncludePlayer)
    {
        foreach (Projectile p in SpawnedProj)
        {
            if(p != null)
            {
                if (p.IsPlayer && IncludePlayer)
                    Destroy(p.gameObject);
                else if (!p.IsPlayer)
                    Destroy(p.gameObject);
            }

        }
        DontSpawn = true;
        Invoke("ReEnable", 1.75f);
    }

    public void ReEnable()
    {
        DontSpawn = false;
    }
}
