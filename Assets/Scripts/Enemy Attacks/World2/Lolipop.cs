using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lolipop : MonoBehaviour
{
    public GameObject Explosion;
    public float AttackTimer;
    Animator LolliPopGFX;
    public Transform SpawnPos;

    void Start()
    {
        LolliPopGFX = GetComponentInChildren<Animator>();
        AttackTimer = Random.Range(5f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        AttackTimer -= Time.deltaTime;
        if (AttackTimer > 0f)
        {
            LolliPopGFX.SetBool("Attack", false);
            LolliPopGFX.SetBool("Walking", GetComponent<Enemies>().PlayerFound);
        }

        if (AttackTimer <= 0 && !GetComponent<Enemies>().InDazedState)
        {
            Vector2 LookDir = Movement.Instance.transform.position - SpawnPos.position;
            float Ang = Mathf.Atan2(LookDir.y, LookDir.x) * Mathf.Rad2Deg - 90f;
            SpawnPos.rotation = Quaternion.Euler(0f, 0f, Ang);
            LolliPopGFX.SetBool("Attack", true);
            AttackTimer = Random.Range(5f, 10f);
        }
    }
}
