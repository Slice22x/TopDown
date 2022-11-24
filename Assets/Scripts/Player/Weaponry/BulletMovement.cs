using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletMovement : Projectile
{
    public Rigidbody2D Rig;
    public int Damage = 0;
    private int Penatration;

    public float Speed;
    public bool DynamicSpeed;
    public AnimationCurve SpeedCurve;
    [HideInInspector] public float SpeedTime;
    public float SpeedMultiply;
    public Vector2 Dir;

    public float DestroyTime;
    public bool StartDestroy = true;

    public TypeOfBullet Bull;
    [HideInInspector] public TypeOfEnemy Enem;
    [HideInInspector] public TypeOfWeapon Weapon;
    [HideInInspector] public int DamageIncrease;

    [HideInInspector] public Enemies.Effect Casue;
    [HideInInspector] public int ElementDamage;
    [HideInInspector] public float DebuffTime;
    [HideInInspector] public float Multiplier;
    [HideInInspector] public float DamageRate;
    public GameObject HitPointText;
    [HideInInspector] public bool HasCritted;

    public TypeOfEnemy Style;

    public enum DeathMeth
    {
        Destroy, // Destroy gameobject
        Spawn, // Spawn an item or something special
        LingerSpawn, // Become a permanent spawner
        BulletHellSpawn, // Spawn A few bullets then die
    }

    [Tooltip("Destroy = Destroys the gameobject when it's time\n" +
    "Spawn = Spawn an item or something special then dies\n" +
    "LingerSpawn = Permanently spawns bullets only and dies at the end of the level\n" +
    "BulletHellSpawn = Spawns a few bullets only and dies")]
    public DeathMeth Method;
    public GameObject Turret;
    public BulletHellTurret TurretOnMe;
    public GameObject ToSpawn;

    protected GameObject EffectObj;

    void Start()
    {
        EffectObj = (GameObject)Resources.Load("Small_Explosion", typeof(GameObject));
        Rig = GetComponent<Rigidbody2D>();
        if (IsPlayer)
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
        }
    }

    private void Update()
    {
        if(Damage < 0)
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
        if (collision.collider.CompareTag("Enemy"))
        {
            Enem = collision.collider.gameObject.GetComponent<Enemies>().EnemyStyler;
            var Enemy = collision.collider.gameObject.GetComponent<Enemies>();
            if (Enemy.InDazedState)
            {
                Enemy.DazedTimer -= (float)Damage / 4f;
            }
            if (!Enemy.InDazedState)
            {
                Damage -= Mathf.RoundToInt((float)Damage * (float)Enem.Defence / 100f);
                Enemy.Health -= Damage;
                Enemy.CurrectEffect = Casue;
                Enemy.MyDuration = DebuffTime;
                Enemy.MyMultiplier = Multiplier;
                Enemy.MyDamageRate = DamageRate;
                Enemy.CurrentElementDamage = ElementDamage;
                if(TextCounter.Instance == null)
                {
                    Instantiate((GameObject)Resources.Load("HitPointParent", typeof(GameObject)), transform.position, Quaternion.identity);
                }
                if(TextCounter.Instance != null)
                {
                    TextCounter.Instance.Acum(Damage, HasCritted);
                    TextCounter.Instance.transform.position = collision.GetContact(0).point;
                }
                if (Enem = null)
                {
                    Debug.LogWarning("There is no enemies Script");
                }
            }

        }

        if (collision.collider.CompareTag("Portal"))
        {
            var PortHealth = collision.collider.gameObject.GetComponent<PortalHealth>();
            PortHealth.Health -= Damage;
            if (TextCounter.Instance == null)
            {
                Instantiate((GameObject)Resources.Load("HitPointParent", typeof(GameObject)), transform.position, Quaternion.identity);
            }
            if (TextCounter.Instance != null)
            {
                TextCounter.Instance.Acum(Damage, HasCritted);
                TextCounter.Instance.transform.position = collision.GetContact(0).point;
            }
        }

        if (collision.collider.CompareTag("Boss"))
        {
            var BossHealth = collision.collider.GetComponentInParent<BossHealth>();
            BossHealth.CurrentHealth -= Damage;
            if (TextCounter.Instance == null)
            {
                Instantiate((GameObject)Resources.Load("HitPointParent", typeof(GameObject)), transform.position, Quaternion.identity);
            }
            if (TextCounter.Instance != null)
            {
                TextCounter.Instance.Acum(Damage, HasCritted);
                TextCounter.Instance.transform.position = collision.GetContact(0).point;
            }
        }

        if (collision.collider.CompareTag("Player"))
        {
            if (!collision.gameObject.GetComponent<Health>().Invis)
            {
                collision.collider.GetComponent<Health>().DealDamage(Style.Damage, false);

            }

        }

        GameObject NewFlash = Instantiate(EffectObj, transform.position, Quaternion.identity);
        ParticleSystem.ShapeModule NewShape = NewFlash.GetComponentInChildren<ParticleSystem>().shape;
        NewShape.rotation = new Vector3(transform.rotation.z + 180, FormulaCalculator.DirectionR(collision.contacts[0].point,transform.position) ? 270f : 90f , FormulaCalculator.DirectionR(collision.contacts[0].point, transform.position) ? 270f : 90f);
        CallOnDie();
    }

    public void CallOnDie()
    {
        switch (Method)
        {
            case DeathMeth.Destroy:
                Destroy(gameObject);
                break;
            case DeathMeth.BulletHellSpawn:
                GameObject NTurret = Instantiate(Turret, transform.position, Quaternion.identity);
                NTurret.GetComponent<BulletHellTurret>().StartCoroutine(NTurret.GetComponent<BulletHellTurret>().Shoot(3, false, 10, 0.3f, 2f, 15f, BulletHellTurret.SpreadTypes.Normal, () => { Destroy(NTurret); }, 0.5f));
                Destroy(gameObject);
                break;
            case DeathMeth.LingerSpawn:
                if (DestroyTime > 0f) { Destroy(gameObject); }
                else
                {
                    TurretOnMe.Shoot();
                }
                break;
            case DeathMeth.Spawn:
                Instantiate(ToSpawn, transform.position, Quaternion.identity);
                Destroy(gameObject);
                break;
        }
    }

}
