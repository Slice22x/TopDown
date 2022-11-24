using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LolipopGFX : MonoBehaviour
{
    Lolipop Parent;
    void Start()
    {
        Parent = GetComponentInParent<Lolipop>();
    }

    public void FreezePosition()
    {
        GetComponentInParent<Enemies>().DontSearch = true;
        Parent.GetComponent<Collider2D>().enabled = false;
    }

    public void UnFreezePosition()
    {
        GetComponentInParent<Enemies>().DontSearch = false;
        Parent.GetComponent<Collider2D>().enabled = true;
    }

    public void SpawnExplosion()
    {
        ExplosionScript.CreateExplosion(Parent.Explosion, Parent.SpawnPos.position, 1f, Parent.GetComponent<Enemies>().Damage,true, Health.EffectType.Fire);
    }
}
