using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CandyCane : MonoBehaviour
{
    public GameObject Explosion;
    public float Distance;
    public float Radius;
    float Spaceing;
    public float WaitBefore;
    public float AttackTimer;
    Animator CandyGFX;
    public Transform SpawnPos;
    void Start()
    {
        CandyGFX = GetComponentInChildren<Animator>();
        AttackTimer = Random.Range(5f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        Spaceing = Radius * 6;
        AttackTimer -= Time.deltaTime;
        if(AttackTimer > 0f)
        {
            CandyGFX.SetBool("Attack", false);
            CandyGFX.SetBool("Walking", GetComponent<Enemies>().PlayerFound);
        }

        if (AttackTimer <= 0 && !GetComponent<Enemies>().InDazedState)
        {
            Vector2 LookDir = Movement.Instance.transform.position - SpawnPos.position;
            float Ang = Mathf.Atan2(LookDir.y, LookDir.x) * Mathf.Rad2Deg - 90f;
            SpawnPos.rotation = Quaternion.Euler(0f, 0f, Ang);
            CandyGFX.SetBool("Attack", true);
            GetComponent<AIPath>().enabled = false;
            AttackTimer = Random.Range(5f, 10f);
        }
    }

    public IEnumerator SpawnExp()
    {
        for (int i = 0; i < Mathf.RoundToInt(Distance / Radius); i++)
        {
            ExplosionScript.CreateExplosion(Explosion, SpawnPos.position + SpawnPos.up * ((i + 1) * (Radius + Spaceing)), Radius, 30, true, Health.EffectType.None);
            if (i > Mathf.RoundToInt(Distance / Radius))
            {
                CandyGFX.SetBool("Attack", false);
                GetComponent<AIPath>().enabled = true;
                StopCoroutine(SpawnExp());
            }
            yield return new WaitForSeconds(WaitBefore);
        }
    }
}
