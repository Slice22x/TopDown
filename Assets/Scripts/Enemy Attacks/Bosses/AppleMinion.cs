using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleMinion : MonoBehaviour
{
    public GameObject Explosion;
    public float WaitTime = 3f;
    bool HasSpawned = false;
    void Start()
    {
        HasSpawned = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (HasSpawned)
        {
            WaitTime -= Time.deltaTime;
            if(WaitTime <= 0)
            {
                ExplosionScript.CreateExplosion(Explosion, transform.position, 1f, 100, true, Health.EffectType.None);
                Destroy(gameObject);
            }
        }
    }
}
