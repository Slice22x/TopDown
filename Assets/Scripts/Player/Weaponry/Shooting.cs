using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [HideInInspector] public GameObject Bullet;
    public GameObject Injection;
    public GameObject OldBullet;
    public Transform FirePoint;


    [HideInInspector] public float BulletSpeed;

    [HideInInspector] public float FireRate = 1f;
    [HideInInspector] public float NextFireTime;
    [HideInInspector]
    public GameObject ReloadIndicator;

    public TypeOfWeapon Weapon;
    [HideInInspector] public bool IsOnPlayer = false;

    [HideInInspector]
    public CinemachineVirtualCamera VCam;
    [HideInInspector] public float Zoom;

    [HideInInspector] public int Ammo;
    [HideInInspector] public int AmmoNeeded;
    [HideInInspector] public int MaxAmmo;
    [HideInInspector] public bool IsEmpty;
    [HideInInspector]
    public float ReloadTime;
    [HideInInspector]
    public int Rounds;
    [HideInInspector]
    public int AmmoUsed;
    [HideInInspector] public Text AmmoHolder;
    [HideInInspector] public Text MaxHolder;
    [HideInInspector] public bool IsReloading;
    [HideInInspector] public int FixedMaxAmmo;

    int NumberOfLoops;
    [HideInInspector] public GameObject[] Objects;
    [HideInInspector] public List<Item> Items;
    [HideInInspector] public float MinSpreadMinus,MaxSpreadMinus;
    bool ItemsAdded;
    bool PlayerAdded;
    int Number;

    [HideInInspector] public ThowingKnife Knife;

    void Start()
    {
        LoadEveryThing();
    }

    // Update is called once per frame
    void Update()
    {
        if(MaxAmmo > FixedMaxAmmo)
        {
            MaxAmmo = FixedMaxAmmo;
        }


        if (IsOnPlayer)
        {
            AddItems();
            ApplyItems();

            ChangeInjection();

            if (!IsEmpty)
            {

                if (Weapon.Automatic)
                {
                    if (Time.time >= NextFireTime)
                    {
                        if (Mouse.current.leftButton.isPressed)
                        {
                            Shoot();
                            NextFireTime = Time.time + 1f / FireRate;
                        }
                    }
                }
                if (!Weapon.Automatic)
                {
                    if (Time.time >= NextFireTime)
                    {
                        if(Mouse.current.leftButton.wasPressedThisFrame)
                        {
                            Shoot();
                            NextFireTime = Time.time + 1f / FireRate;
                        }
                    }
                }
                

                
            }

            AmmoHolder.text = Ammo.ToString();
            MaxHolder.text = MaxAmmo.ToString();

            if(Weapon.Zoom > 0)
            {
               VCam.m_Lens.OrthographicSize = Mathf.Lerp(VCam.m_Lens.OrthographicSize,Weapon.Zoom, 0.125f);
            }
            if (Weapon.Zoom < 0)
            {
                VCam.m_Lens.OrthographicSize = Mathf.Lerp(VCam.m_Lens.OrthographicSize, Weapon.Zoom, 0.125f);
            }
            if (Weapon.Zoom == 0)
            {
                VCam.m_Lens.OrthographicSize = Mathf.Lerp(VCam.m_Lens.OrthographicSize, 10, 0.125f);
            }
        }




        if (transform.parent == Movement.Instance.transform.GetChild(0))
        {
            IsOnPlayer = true;
        }

        Reload();


    }

    protected void ChangeInjection()
    {
        Knife = GetComponentInParent<ThowingKnife>();
        if (Knife.CanThrowEnemy)
        {
            Bullet = Injection;
            if (Mouse.current.leftButton.wasPressedThisFrame && !Knife.Script)
            {
                Knife.Script.Injected = true;
            }
        }
        if (!Knife.CanThrowEnemy)
        {
            Bullet = OldBullet;
        }
    }

    public virtual void Reload()
    {
        if (SceneType.Instance.ThisScene == SceneType.TypeOfScene.Village)
            Destroy(gameObject);
        if (!Weapon.UseAmmo)
            return;

        if (Ammo <= 0)
        {
            IsEmpty = true;
        }
        if (IsEmpty && MaxAmmo > 0 || Keyboard.current.rKey.wasPressedThisFrame)
        {
            IsReloading = true;
        }
        if (IsReloading)
        {
            ReloadIndicator.GetComponent<Image>().color = new Color(255f,255f,255f,255f);
            ReloadIndicator.GetComponent<Animator>().SetBool("Reloading", true);
            ReloadTime -= Time.deltaTime;
            if (ReloadTime <= 0)
            {
                IsReloading = false;
                AmmoUsed = AmmoNeeded - Ammo;
                MaxAmmo -= AmmoUsed;
                Ammo = AmmoNeeded;
                IsEmpty = false;
                ReloadTime = Weapon.RealoadTime;
                ReloadIndicator.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
                ReloadIndicator.GetComponent<Animator>().SetBool("Reloading", false);
            }
        }
    }

    protected void LoadEveryThing()
    {
        FireRate = Weapon.FireRate;
        Bullet = OldBullet;
        BulletSpeed = Weapon.ExtraSpeed + Weapon.Bull.BaseSpeed;
        VCam = GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>();
        Zoom = Weapon.Zoom;
        Rounds = Weapon.Rounds;
        Ammo = Weapon.Ammo;
        MaxAmmo = Ammo * Rounds;
        ReloadTime = Weapon.RealoadTime;
        AmmoNeeded = Ammo;
        FixedMaxAmmo = MaxAmmo;
        ReloadIndicator = GameObject.FindGameObjectWithTag("Reload");
        AmmoHolder = GameObject.FindGameObjectWithTag("AmmoHolder").GetComponent<Text>();
        MaxHolder = GameObject.FindGameObjectWithTag("MaxHolder").GetComponent<Text>();
        OldBullet = Bullet;

    }

    public virtual void AddItems()
    {
        Movement.Instance.GunInHand = this;
        if (!PlayerAdded)
        {
            Items.Clear();
            Objects = Movement.Instance.ItemsInHand.ToArray();
            PlayerAdded = true;
        }
        if (Objects != null)
        {
            if (!ItemsAdded)
            {
                for (int i = 0; i < Objects.Length; i++)
                {
                    Items.Add(Objects[i].GetComponent<Item>());
                    Number++;
                    if (Number >= Objects.Length)
                    {
                        ItemsAdded = true;
                    }
                }
            }
        }
    }

    public virtual void ApplyItems()
    {
        if (Items != null)
        {
            if (NumberOfLoops < Items.Count)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    BulletMovement BullMovement = Bullet.GetComponent<BulletMovement>();
                    BullMovement.DamageIncrease = Items[i].ItemOfChoice.DamageAdder;
                    Zoom += Items[i].ItemOfChoice.ZoomAdder;
                    BulletSpeed += Items[i].ItemOfChoice.BulletSpeedAdder;
                    MinSpreadMinus = Items[i].ItemOfChoice.MinSpreadMinus;
                    MaxSpreadMinus = Items[i].ItemOfChoice.MaxSpreadMinus;


                    BullMovement.Casue = Items[i].ItemOfChoice.CauseEffect;

                    BullMovement.ElementDamage = Items[i].ItemOfChoice.ElementDamage;
                    BullMovement.DebuffTime = Items[i].ItemOfChoice.DebufTimeAdder;
                    BullMovement.Multiplier = Items[i].ItemOfChoice.DebufMultiplierAdder;
                    BullMovement.DamageRate = Items[i].ItemOfChoice.DamageRate;
                    NumberOfLoops++;
                }
            }

        }
    }

    public virtual void RightClickAbility()
    {
        //better accuracy
    }

    public virtual void Shoot()
    {
        GameObject ShootBoi = Instantiate(Bullet, FirePoint.position, FirePoint.rotation);
        Rigidbody2D Rigs = ShootBoi.GetComponent<Rigidbody2D>();
        var Spread = UnityEngine.Random.Range(Weapon.MinSpread -= MinSpreadMinus, Weapon.MaxSpread -= MaxSpreadMinus);
        if (Bullet == OldBullet)
        {
            Ammo--;
        }
        if (ShootBoi.GetComponent<BulletMovement>() != null)
        {
            ShootBoi.GetComponent<BulletMovement>().Weapon = Weapon;
            ShootBoi.GetComponent<BulletMovement>().Speed = BulletSpeed;
            ShootBoi.GetComponent<BulletMovement>().SpeedMultiply = BulletSpeed;
            ShootBoi.GetComponent<BulletMovement>().Dir = FormulaCalculator.Direction(Camera.main.ScreenToWorldPoint((Mouse.current.position.ReadValue() + (UnityEngine.Random.insideUnitCircle * Spread))), FirePoint.position);
        }
        CinemachineShake.Instance.ShakeCamera(2.5f, 0.1f);
        SoundManager.Play("Shoot");
        PortalInfo.Instance.ThisLevel.BulletsShot++;
    }
}
