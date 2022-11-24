using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Detection))]
public class Banana : MonoBehaviour
{
    private Detection Detect;
    private Transform Player;
    public GameObject Explosion;
    public TypeOfEnemy Enem;

    private float s;

    Rigidbody2D Body;
    Vector2 PlayerPosVec2;
    void Start()
    {
        Body = GetComponent<Rigidbody2D>();
        Player = Movement.Instance.transform;
        Detect = GetComponent<Detection>();
        Detect.DetectionRange = 2f;
        Launch();
    }

    // Update is called once per frame
    void Launch()
    {
        s = Enem.Speed;
        PlayerPosVec2 = Player.position;
        Vector2 LookDir = PlayerPosVec2 - Body.position;
        float Ang = Mathf.Atan2(LookDir.y, LookDir.x) * Mathf.Rad2Deg - 90f;
        Body.rotation = Ang;
    }

    private void Update()
    {
        if (Detect.Detected)
        {
            ExplosionScript.CreateExplosion(Explosion, transform.position, 1f, 100, true, Health.EffectType.None);
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ExplosionScript.CreateExplosion(Explosion, transform.position, 1f, 100, true, Health.EffectType.None);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        Body.AddForce(transform.up * s * Time.deltaTime, ForceMode2D.Impulse);
    }
}
