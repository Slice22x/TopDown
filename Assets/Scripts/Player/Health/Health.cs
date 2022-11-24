using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using TMPro;

public class Health : MonoBehaviour
{
    public int Hearts;
    public int NumberOfHearts;
    public int HP;
    public int MaxHP;

    public bool Invis;
    public float InvisTime;

    float duration, DamageRate, NextDamageRate;
    int Damage;

    Vignette V;

    [HideInInspector] public bool Wait;

    public bool Invicible;

    public enum EffectType
    {
        None,
        Fire,
        Ice,
        Poison
    }

    public EffectType CurrentEffect;

    public static Health Instance;

    private void Awake()
    {
        Instance = this;
        MaxHP = HP;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

    }

    private void Start()
    {
        if (SceneType.Instance.ThisScene == SceneType.TypeOfScene.Village)
            return;
        MaxHP = HP;
        Instance = this;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        CinemachineShake.Instance.Vol.profile.TryGet(out V);
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (SceneType.Instance.ThisScene == SceneType.TypeOfScene.Village)
            return;
        Instance = this;
        MaxHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Wait)
        {
            if (WeaponManager.Instance.CurrentWeapon != null)
            {
                if (WeaponManager.Instance.CurrentWeapon.Weapon.GunStyle != TypeOfWeapon.Style.Charge)
                {
                    V.intensity.value = Mathf.Lerp(V.intensity.value, 0f, Time.smoothDeltaTime * 15f);
                }
            }
            else
            {
                V.intensity.value = Mathf.Lerp(V.intensity.value, 0f, Time.smoothDeltaTime * 15f);
            }
        }


        if (HP <= 0)
        {
            Hearts--;
            HP = MaxHP;
        }

        if(HP > MaxHP && Hearts < NumberOfHearts)
        {
            Hearts++;
            HP -= MaxHP;
        }
        else if(HP > MaxHP && Hearts >= NumberOfHearts)
        {
            HP = MaxHP;
        }
        if(Hearts < 0)
        {
            PortalInfo.Instance.ThisLevel.Deaths++;
            Destroy(gameObject);
            FindObjectOfType<DeathScreen>().CallDeath();
            Cursor.visible = true;
        }

        if (Invis == true)
        {
            InvisTime -= Time.deltaTime;
            if (InvisTime <= 0)
            {
                Invis = false;
                InvisTime = 3;
            }
        }

        if(CurrentEffect != EffectType.None)
        {
            duration -= Time.deltaTime;
            if (duration > 0f)
            {
                if(Time.time >= NextDamageRate)
                {
                    HP -= Damage;
                    GetComponentInParent<Movement>().Rend.color = Color.red;
                    SoundManager.Play("Hurt");
                    Invoke("NormalColour", .3f);
                    PortalInfo.Instance.ThisLevel.DamageTaken += Damage;
                    NextDamageRate = Time.time + 1f / DamageRate;
                }
            }
            else if (duration <= 0f)
            {
                EffectUIManager.Instance.CurrentEffect = EffectType.None;
                CurrentEffect = EffectType.None;
            }
        }
    }

    public void NormalColour()
    {
        GetComponentInParent<Movement>().Rend.color = Color.white;
        Wait = false;
    }

    public void DealDamage(int Damage, bool CauseInvis)
    {
        if (Invicible) return;

        switch (CurrentEffect)
        {
            case EffectType.None:
                V.color.value = new Color(175f, 0f, 0f, 255f);
                break;
            case EffectType.Poison:
                V.color.value = new Color(125f,0f,125f,255f);
                break;
        }

        if (!Invis)
        {
            HP -= Damage;
            Invis = CauseInvis;
        }
        if (CauseInvis)
        {
            V.intensity.value = 0.24f;
        }
        else
        {
            V.intensity.value += 0.16f;
        }
        Wait = true;
        V.smoothness.value = 0.01f;
        GetComponentInParent<Movement>().Rend.color = Color.red;
        Invoke("NormalColour", .4f);
        SoundManager.Play("Hurt");
        PortalInfo.Instance.ThisLevel.DamageTaken += Damage;
    }

    public void CauseEffect(EffectType Type, int Damage, float DamageRate, float Duration)
    {
        duration = Duration;
        EffectUIManager.Instance.CurrentEffect = Type;
        EffectUIManager.Instance.Duration = Duration;
        this.DamageRate = DamageRate;
        CurrentEffect = Type;
        this.Damage = Damage;
    }

}
