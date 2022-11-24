using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class ChargeUp : Shooting
{
    public GameObject ChargeBullet;
    public float ChargeValue;
    public ParticleSystem DragIn;

    LensDistortion LD;
    ChromaticAberration CD;
    Vignette V;

    void Start()
    {
        LoadEveryThing();
        CinemachineShake.Instance.Vol.profile.TryGet(out LD);
        CinemachineShake.Instance.Vol.profile.TryGet(out CD);
        CinemachineShake.Instance.Vol.profile.TryGet(out V);
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
                if (!IsReloading)
                {
                    if (Time.time >= NextFireTime)
                    {
                        if (Mouse.current.leftButton.isPressed)
                        {
                            Shoot();
                            NextFireTime = Time.time + 1f / Weapon.ChargeRate;
                        }

                        if (!Mouse.current.leftButton.isPressed)
                        {
                            Release();
                            if (!Health.Instance.Wait)
                            {
                                V.color.value = new Color(4f, 9f, 60f);
                                LD.intensity.value = Mathf.Lerp(LD.intensity.value, 0f, Time.smoothDeltaTime * 15f);
                                CD.intensity.value = Mathf.Lerp(CD.intensity.value, 0f, Time.smoothDeltaTime * 15f);
                                V.intensity.value = Mathf.Lerp(V.intensity.value, 0f, Time.smoothDeltaTime * 15f);
                            }
                            NextFireTime = Time.time + 1f / Weapon.ChargeRate;
                        }
                    }
                }
            }
            if (IsEmpty)
            {
                if(ChargeBullet != null)
                {
                    Release();
                }
            }
            if(ChargeValue > Weapon.ChargeMax)
            {
                ChargeValue = Weapon.ChargeMax;
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
            Reload();
        }
        if (transform.parent == Movement.Instance.transform.GetChild(0))
        {
            IsOnPlayer = true;
        }
    }

    void Release()
    {
        if (ChargeBullet != null)
        {
            ChargeBullet.GetComponent<BulletMovement>().StartDestroy = true;
            Rigidbody2D Rigs = ChargeBullet.GetComponent<Rigidbody2D>();
            ChargeBullet.GetComponent<Collider2D>().enabled = true;
            Rigs.isKinematic = false;
            ChargeBullet.transform.parent = null;
            ChargeBullet.GetComponent<BulletMovement>().Speed = BulletSpeed;
            ChargeBullet.GetComponent<BulletMovement>().SpeedMultiply = BulletSpeed;
            ChargeBullet.GetComponent<BulletMovement>().Dir = FormulaCalculator.Direction(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), FirePoint.position);
            if (ChargeBullet.GetComponent<BulletMovement>() != null)
            {
                ChargeBullet.GetComponent<BulletMovement>().Weapon = Weapon;
                ChargeBullet.GetComponent<BulletMovement>().Damage = Mathf.RoundToInt(Weapon.Bull.BaseDamage * ChargeValue / 10f);
            }
            ChargeValue = 0f;
            CinemachineShake.Instance.ShakeCamera(7f, 0.1f);
            ChargeBullet = null;
            SoundManager.Play("BigExp");
            PortalInfo.Instance.ThisLevel.BulletsShot++;
        }

    }

    public override void Shoot()
    {
        if(Bullet != Injection)
        {
            if (ChargeBullet == null)
            {
                ChargeBullet = Instantiate(OldBullet, FirePoint.position, Quaternion.identity, FirePoint);
                ChargeBullet.GetComponent<Rigidbody2D>().isKinematic = true;
                ChargeBullet.GetComponent<BulletMovement>().StartDestroy = false;
            }
            if (ChargeBullet != null)
            {
                if (Ammo > 0 && ChargeValue < Weapon.ChargeMax)
                {
                    ChargeValue += Time.smoothDeltaTime * 200f;
                    ChargeBullet.transform.localScale = new Vector3(ChargeValue / 20f + 0.5f, ChargeValue / 20f + 0.5f);
                    V.color.value = new Color(4f, 9f, 60f);
                    V.smoothness.value = 1f;
                    LD.intensity.value -= Time.smoothDeltaTime / 2;
                    CD.intensity.value += Time.smoothDeltaTime;
                    V.intensity.value += Time.smoothDeltaTime / 2;
                    if (Random.value > 0.5 && Bullet == OldBullet)
                    {
                        Ammo--;
                    }
                    if(ChargeValue > 0.2 * Weapon.ChargeMax)
                    {
                        ChargeBullet.GetComponent<Collider2D>().enabled = false;
                    }
                }
            }
        }
    }

}
