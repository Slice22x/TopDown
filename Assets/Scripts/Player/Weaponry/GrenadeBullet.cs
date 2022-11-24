using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrenadeBullet : BulletMovement
{
    public GameObject Explosion;
    private float WaitToExp;
    void Start()
    {
        DestroyTime = Bull.BaseRange + Weapon.ExtraRange;
        if (Random.value < Bull.CriticalChance)
        {
            if (DamageIncrease < 0)
            {
                Damage = (Bull.BaseDamage + Weapon.ExtraDamage + DamageIncrease) * Bull.CriticalMultiplier;
                //Debug.LogWarning("HasCrit");
            }
            else
            {
                Damage = (Bull.BaseDamage + Weapon.ExtraDamage) * Bull.CriticalMultiplier;
                // Debug.LogWarning("HasCrit");
            }
            HasCritted = true;
        }
        else
        {
            if (DamageIncrease < 0)
            {
                Damage = Bull.BaseDamage + Weapon.ExtraDamage + DamageIncrease;
            }
            else
            {
                Damage = Bull.BaseDamage + Weapon.ExtraDamage;
            }
        }
        Rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Damage < 0)
        {
            Damage = Bull.BaseDamage;
        }
        if (DynamicSpeed)
        {
            SpeedTime += Time.deltaTime / 10;
            Speed = SpeedMultiply * SpeedCurve.Evaluate(SpeedTime);
            Rig.velocity = (Dir * Speed);
        }
        if (!DynamicSpeed)
        {
            Rig.velocity = (Dir * Speed);
        }
        if (StartDestroy)
            DestroyTime -= Time.deltaTime;
        if (DestroyTime <= 0f)
        {
            CallOnDie();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Enemy")
        {
            Enem = collision.collider.gameObject.GetComponent<Enemies>().EnemyStyler;
            var Enemy = collision.collider.gameObject.GetComponent<Enemies>();
            if (Enemy.InDazedState)
                return;
            ExplosionScript.CreateExplosion(Explosion, transform.position, 1f, Damage, false, Health.EffectType.None);
            Enemy.CurrectEffect = Casue;
            Enemy.MyDuration = DebuffTime;
            Enemy.MyMultiplier = Multiplier;
            Enemy.MyDamageRate = DamageRate;
            Enemy.CurrentElementDamage = ElementDamage;
            GameObject TextHit = Instantiate(HitPointText, transform.position, Quaternion.identity);
            TextHit.GetComponentInChildren<TMP_Text>().text = Damage.ToString();
            if (HasCritted)
            {
                TextHit.GetComponentInChildren<TMP_Text>().outlineColor = Color.white;
                TextHit.GetComponentInChildren<TMP_Text>().color = Color.red;
            }
            Destroy(gameObject);
        }
        else
        {
            ExplosionScript.CreateExplosion(Explosion, transform.position, 1f, Damage, false, Health.EffectType.None);
            Destroy(gameObject);
        }
    }
}
