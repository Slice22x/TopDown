using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public GameObject HitPointText;
    public bool Enemy;
    public int Damage;

    public Health.EffectType EffectType;

    [SerializeField] private GameObject ExplosionCircle;
    [SerializeField] private AnimationCurve ExplosionCurve;

    private void Start()
    {
        CinemachineShake.Instance.ShakeCamera(5f, 1f);
        LeanTween.scale(ExplosionCircle, new Vector3(1000f, 1000f), 0.55f).setEase(ExplosionCurve);
    }

    private void Update()
    {
        Destroy(gameObject, 0.95f);
    }

    public static void CreateExplosion(GameObject Explosion, Vector3 Position, float Size, int Damage, bool EnemyExplosion, Health.EffectType effectType)
    {
        SoundManager.Play("Exp");
        GameObject Spawned = Instantiate(Explosion, Position, Quaternion.identity);
        Spawned.transform.localScale = new Vector3(Size, Size);
        Spawned.GetComponent<ExplosionScript>().Damage = Damage;
        Spawned.GetComponent<ExplosionScript>().Enemy = EnemyExplosion;
        Spawned.GetComponent<ExplosionScript>().EffectType = effectType;
    }

    public static void CreateBigExplosion(GameObject Explosion, Vector3 Position, float Size, int Damage, bool EnemyExplosion, Health.EffectType effectType)
    {
        SoundManager.Play("BigExp");
        GameObject Spawned = Instantiate(Explosion, Position, Quaternion.identity);
        Spawned.transform.localScale = new Vector3(Size, Size);
        Spawned.GetComponent<ExplosionScript>().Damage = Damage;
        Spawned.GetComponent<ExplosionScript>().Enemy = EnemyExplosion;
        Spawned.GetComponent<ExplosionScript>().EffectType = effectType;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                if(!Enemy)
                    collision.gameObject.GetComponent<Enemies>().Health -= Damage;
                break;
            case "Player":
                Health.Instance.DealDamage(100,true);
                GameObject TextHit = Instantiate(HitPointText, transform.position, Quaternion.identity);
                TextHit.GetComponentInChildren<TMP_Text>().text = Damage.ToString();
                if(EffectType != Health.EffectType.None)
                {
                    Health.Instance.CauseEffect(EffectType, 10, 2f, 5f);
                }
                break;
            case "Portal":
                Debug.Log("Portal");
                collision.GetComponent<PortalHealth>().Health -= 100;
                break;
        }
    }
}
