using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int AmmoSupply;

    bool CanApply = false;

    private void Update()
    {
        if (CanApply)
        {
            SoundManager.Play("PowerA");
            Destroy(gameObject);
            Movement.Instance.AmmoSystem.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            switch (collision.GetComponentInChildren<Shooting>().Weapon.Required)
            {
                case TypeOfWeapon.RequiredAmmo.AKBullet:
                    AmmoSupply = Random.Range(50, 100);
                    break;
                case TypeOfWeapon.RequiredAmmo.MiniBullet:
                    AmmoSupply = Random.Range(30, 50);
                    break;
                case TypeOfWeapon.RequiredAmmo.MachineBullet:
                    AmmoSupply = Random.Range(75, 300);
                    break;
                case TypeOfWeapon.RequiredAmmo.Rockets:
                    AmmoSupply = Random.Range(1, 10);
                    break;
                case TypeOfWeapon.RequiredAmmo.ShotgunBullet:
                    AmmoSupply = Random.Range(20, 100);
                    break;
                case TypeOfWeapon.RequiredAmmo.SniperBullet:
                    AmmoSupply = Random.Range(1, 10);
                    break;
                case TypeOfWeapon.RequiredAmmo.Laser:
                    AmmoSupply = Random.Range(100, 200);
                    break;
                case TypeOfWeapon.RequiredAmmo.SpecialBullets:
                    AmmoSupply = Random.Range(50, 100);
                    break;
                case TypeOfWeapon.RequiredAmmo.RocketFuel:
                    AmmoSupply = Random.Range(50, 100);
                    break;
                default:
                    AmmoSupply = 0;
                    Debug.LogError("No WeaponType");
                    break;
            }

            CanApply = true;
            WeaponManager.Instance.CurrentWeapon.MaxAmmo += AmmoSupply;
        }
    }
}
