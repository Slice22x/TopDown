using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{

    public int CurrentHealth;
    private int MaxHealth;
    public float HealthPercent;
    public HealthBar Bar;
    public string BossName;

    public delegate void OnDeath();
    public static event OnDeath Death;

    Apple Fruit;

    void Start()
    {
        CurrentHealth = Mathf.RoundToInt(Movement.Instance.Info.RepModifiyer(CurrentHealth, !Movement.Instance.Info.VillageSide()));
        MaxHealth = CurrentHealth;
        Bar.SetMaxValue(MaxHealth);
        Fruit = GetComponentInChildren<Apple>();
    }

    // Update is called once per frame
    void Update()
    {
        HealthPercent = ((float)CurrentHealth / (float)MaxHealth) * 100;

        string Vals = BossName + ":" + " " + CurrentHealth.ToString() + "/" + MaxHealth.ToString();
        Bar.SetText(Vals);
        Bar.SetValue(CurrentHealth);

        if(HealthPercent <= 50f && !Fruit.Phase2)
        {
            Fruit.StopAllCoroutines();
            Fruit.ReEnable();
            Projectile_Check.Instance.DestroyProjs(true);
            Fruit.Phase2 = true;
        }

        if(CurrentHealth <= 0)
        {
            if(!Fruit.InDenial)
                Fruit.DenialDeath();
        }
    }
    
    public void CallDie()
    {
        if (Death != null)
            Death.Invoke();
        Projectile_Check.Instance.DestroyProjs(false);
    }
}
