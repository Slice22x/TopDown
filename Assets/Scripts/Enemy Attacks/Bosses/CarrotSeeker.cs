using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotSeeker : Projectile
{
    public float Speed;
    public Transform Target;
    Rigidbody2D Body;
    public GameObject Explosion;
    public float Timer = 7f;
    bool Seeking;

    void Start()
    {
        Invoke("KillSelf", Timer);
        Body = GetComponent<Rigidbody2D>();
        Target = Movement.Instance.transform;
        Speed += Random.Range(-2f, 2f);
    }

    private void FixedUpdate()
    {
        Invoke("Seek", .75f);
        Timer -= Time.deltaTime;
        if (Timer < 3f && Seeking)
        {
            Body.angularVelocity = 0f;
        }
        if (Timer >= 3f && Seeking)
        {
            Vector2 LookDir = (Vector2)Target.position - Body.position;
            LookDir.Normalize();
            float RotateAmount = Vector3.Cross(LookDir, -transform.up).z;
            Body.angularVelocity = -RotateAmount * 200f;
        }
        if(Seeking)
            Body.velocity = -transform.up * Speed;
    }

    public void Seek()
    {
        Seeking = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ExplosionScript.CreateExplosion(Explosion, transform.position, 0.5f, 30, true, Health.EffectType.Poison);
        Destroy(gameObject);
    }

    public void KillSelf()
    {
        Destroy(gameObject);
    }
}
