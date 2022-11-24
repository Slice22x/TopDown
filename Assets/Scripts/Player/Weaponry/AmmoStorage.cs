using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoStorage
{
    public int MaxAmmo;
    public int Ammo;

    public void SetAmmo(int MaxAmmo, int Ammo)
    {
        this.MaxAmmo = MaxAmmo;
        this.Ammo = Ammo;
    }

    public void GetAmmo(out int MaxAmmo, out int Ammo)
    {
        MaxAmmo = this.MaxAmmo;
        Ammo = this.Ammo;
    }
}
