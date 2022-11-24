using UnityEngine;
using System.Collections;
using TMPro;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Lazer : Shooting
{

    private LineRenderer LazerPointer;

    public GameObject StartVFX;
    public GameObject EndVFX;
    private int DamageIncrease;
    private int Damage;

    public GameObject HitPointText;

    public LayerMask WhatIsPassable;

    void Start()
    {
        VCam = GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>();
        LazerPointer = GetComponentInChildren<LineRenderer>();
        LazerPointer.enabled = false;
        StartVFX.SetActive(false);
        EndVFX.SetActive(false);
        if (UnityEngine.Random.value < Weapon.Bull.CriticalChance)
        {
            if (DamageIncrease < 0)
            {
                Damage = (Weapon.Bull.BaseDamage + Weapon.ExtraDamage + DamageIncrease) * Weapon.Bull.CriticalMultiplier;
                //Debug.LogWarning("HasCrit");
            }
            else
            {
                Damage = (Weapon.Bull.BaseDamage + Weapon.ExtraDamage) * Weapon.Bull.CriticalMultiplier;
                // Debug.LogWarning("HasCrit");
            }
        }
        else
        {
            if (DamageIncrease < 0)
            {
                Damage = Weapon.Bull.BaseDamage + Weapon.ExtraDamage + DamageIncrease;
            }
            else
            {
                Damage = Weapon.Bull.BaseDamage + Weapon.ExtraDamage;
            }
        }
        FireRate = Weapon.FireRate;
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

    }

    // Update is called once per frame
    void Update()
    {
        if (IsOnPlayer)
        {
            AddItems();
            ApplyItems();
            ChangeInjection();

            if (!IsEmpty)
            {
                if (Mouse.current.leftButton.isPressed)
                {
                    Shoot();
                    StartVFX.SetActive(true);
                    LazerPointer.enabled = true;
                }

                if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    StartVFX.SetActive(false);
                    LazerPointer.enabled = false;
                    EndVFX.SetActive(false);
                }
            }

            AmmoHolder.text = Ammo.ToString();
            MaxHolder.text = MaxAmmo.ToString();

            if (Weapon.Zoom > 0)
            {
                VCam.m_Lens.OrthographicSize = Mathf.Lerp(VCam.m_Lens.OrthographicSize, Weapon.Zoom, 0.125f);
            }
            if (Weapon.Zoom < 0)
            {
                VCam.m_Lens.OrthographicSize = Mathf.Lerp(VCam.m_Lens.OrthographicSize, Weapon.Zoom, 0.125f);
            }
            if (Weapon.Zoom == 0)
            {
                VCam.m_Lens.OrthographicSize = Mathf.Lerp(VCam.m_Lens.OrthographicSize, 8, 0.125f);
            }

        }
        if (transform.parent == Movement.Instance.transform.GetChild(0))
        {
            IsOnPlayer = true;
        }
        Reload();
    }

    public override void Shoot()
    {
        LazerPointer.enabled = true;
        LazerPointer.SetPosition(0, FirePoint.position);
        LazerPointer.SetPosition(1, (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        StartVFX.SetActive(true);



        Vector2 Direct = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - (Vector2)FirePoint.position;
        RaycastHit2D Hit = Physics2D.Raycast(FirePoint.position, Direct.normalized, Direct.magnitude, WhatIsPassable);
        if (Hit)
        {
            LazerPointer.SetPosition(1, Hit.point);
            EndVFX.SetActive(true);
            EndVFX.transform.position = Hit.point;
        }
        if (!Hit)
        {
            EndVFX.SetActive(false);
        }
        if (Time.time >= NextFireTime)
        {
            Ammo--;
            if (Hit)
            {
                if (Hit.collider.tag == "Enemy")
                {
                    Hit.collider.GetComponent<Enemies>().Health -= Damage + ComboSystem.Instance.Combo;
                    GameObject TextHit = Instantiate(HitPointText, Hit.point, Quaternion.identity);
                    TextHit.GetComponentInChildren<TMP_Text>().text = Damage.ToString();

                }
                if (Hit.collider.tag == "Portal")
                {
                    Hit.collider.GetComponent<PortalHealth>().Health -= Damage + ComboSystem.Instance.Combo;
                    GameObject TextHit = Instantiate(HitPointText, Hit.point, Quaternion.identity);
                    TextHit.GetComponentInChildren<TMP_Text>().text = Damage.ToString();
                }
            }
            NextFireTime = Time.time + 1f / FireRate;
        }
        PortalInfo.Instance.ThisLevel.BulletsShot++;
    }
}
