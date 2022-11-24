using UnityEngine;
using System.Collections;
using Cinemachine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Shotgun : Shooting
{

    private float StartAngle, EndAngle;
    List<Transform> Turrets = new List<Transform>();
    public GameObject FireP;
    public int NumberOfPoints;
    void Start()
    {
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

        FirePoint.rotation = Quaternion.Euler(0f, 0f, Weapon.Stableiser);
        StartAngle = Weapon.StartAngle;
        EndAngle = Weapon.EndAngle;
        if (Turrets.Count < NumberOfPoints)
        {
            float AngleStep = (EndAngle - StartAngle) / NumberOfPoints;
            float Angle = StartAngle;

            for (int i = 0; i < NumberOfPoints; i++)
            {
                GameObject NewTurret = Instantiate(FireP, FirePoint.position, Quaternion.Euler(new Vector3(0f, 0f, Angle) - FirePoint.transform.eulerAngles), FirePoint);
                Turrets.Add(NewTurret.transform);
                Angle += AngleStep;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (IsOnPlayer)
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

            AmmoHolder.text = Ammo.ToString();
            MaxHolder.text = MaxAmmo.ToString();

            if (!IsEmpty)
            {
                if (!IsReloading)
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
                            if (Mouse.current.leftButton.wasPressedThisFrame)
                            {
                                Shoot();
                                NextFireTime = Time.time + 1f / FireRate;
                            }
                        }
                    }
                }
            }
        }

        Reload();

        if (transform.parent == Movement.Instance.transform.GetChild(0))
        {
            IsOnPlayer = true;
        }
    }

    public override void Shoot()
    {
        if(Bullet == OldBullet)
        {
            if (Time.time >= NextFireTime)
            {
                foreach (Transform i in Turrets)
                {
                    GameObject ShootBoi = Instantiate(Bullet, i.position, i.rotation);
                    Rigidbody2D Rigs = ShootBoi.GetComponent<Rigidbody2D>();
                    var Spread = Random.Range(Weapon.MinSpread -= MinSpreadMinus, Weapon.MaxSpread -= MaxSpreadMinus);
                    ShootBoi.transform.Rotate(new Vector3(0f, 0f, ShootBoi.transform.rotation.x));
                    Rigs.AddForce(ShootBoi.transform.up * BulletSpeed, ForceMode2D.Impulse);
                    Ammo--;
                    CinemachineShake.Instance.ShakeCamera(2.5f, 0.1f);
                }
                NextFireTime = Time.time + 1f / FireRate;
            }
        }
        if(Bullet == Injection)
        {
            GameObject ShootBoi = Instantiate(Bullet, FirePoint.position, FirePoint.rotation);
            Rigidbody2D Rigs = ShootBoi.GetComponent<Rigidbody2D>();
            var Spread = UnityEngine.Random.Range(Weapon.MinSpread, Weapon.MaxSpread);
            ShootBoi.transform.Rotate(new Vector3(0f, 0f, ShootBoi.transform.rotation.x));
            Rigs.AddForce(ShootBoi.transform.up * BulletSpeed, ForceMode2D.Impulse);
            if (Bullet == OldBullet)
            {
                Ammo--;
            }
            if (ShootBoi.GetComponent<BulletMovement>() != null)
            {
                ShootBoi.GetComponent<BulletMovement>().Weapon = Weapon;
            }
            CinemachineShake.Instance.ShakeCamera(2.5f, 0.1f);
        }
        PortalInfo.Instance.ThisLevel.BulletsShot++;
    }
}
