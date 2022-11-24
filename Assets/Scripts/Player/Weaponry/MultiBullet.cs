using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiBullet : Shooting
{
    public GameObject SpecialBullet;
    [SerializeField]int ShootTimes;
    int RandomRange;
    void Start()
    {
        LoadEveryThing();
        RandomRange = Random.Range(Weapon.EveryNShotsRange.x, Weapon.EveryNShotsRange.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOnPlayer)
        {
            AddItems();
            ApplyItems();
            ChangeInjection();

            if (ShootTimes % Weapon.EveryNShots == 0 && !Knife.CanThrowEnemy && !Weapon.RangeNShots)
            {
                Bullet = SpecialBullet;
            }
            if(ShootTimes % RandomRange == 0 && !Knife.CanThrowEnemy && Weapon.RangeNShots)
            {
                Bullet = SpecialBullet;
            }

            if (!IsEmpty)
            {
                if(Time.time >= NextFireTime)
                {
                    if (Mouse.current.leftButton.isPressed)
                    {
                        Shoot();
                        NextFireTime = Time.time + 1f / FireRate;
                    }
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
        RandomRange = Random.Range(Weapon.EveryNShotsRange.x, Weapon.EveryNShotsRange.y);
        if (ShootTimes % Weapon.EveryNShots == 0 && !Knife.CanThrowEnemy && !Weapon.RangeNShots)
        {
            Bullet = SpecialBullet;
        }
        if (ShootTimes % RandomRange == 0 && !Knife.CanThrowEnemy && Weapon.RangeNShots)
        {
            Bullet = SpecialBullet;
        }
        if (ShootTimes > 0 && ShootTimes % Weapon.EveryNShots != 0 && !Knife.CanThrowEnemy)
        {
            Bullet = OldBullet;
        }   
        GameObject ShootBoi = Instantiate(Bullet, FirePoint.position, FirePoint.rotation);
        Rigidbody2D Rigs = ShootBoi.GetComponent<Rigidbody2D>();
        var Spread = Random.Range(Weapon.MinSpread -= MinSpreadMinus, Weapon.MaxSpread -= MaxSpreadMinus);
        ShootBoi.transform.Rotate(new Vector3(0f, 0f, ShootBoi.transform.rotation.x + Spread));
        ShootBoi.GetComponent<BulletMovement>().Speed = BulletSpeed;
        ShootBoi.GetComponent<BulletMovement>().SpeedMultiply = BulletSpeed;
        ShootBoi.GetComponent<BulletMovement>().Dir = FormulaCalculator.Direction(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), FirePoint.position);
        if (Bullet == OldBullet || Bullet == SpecialBullet)
        {
            Ammo--;
        }
        if (ShootBoi.GetComponent<BulletMovement>() != null)
        {
            ShootBoi.GetComponent<BulletMovement>().Weapon = Weapon;
        }
        CinemachineShake.Instance.ShakeCamera(2.5f, 0.1f);
        SoundManager.Play("Shoot");
        ShootTimes++;
        PortalInfo.Instance.ThisLevel.BulletsShot++;
    }
}
