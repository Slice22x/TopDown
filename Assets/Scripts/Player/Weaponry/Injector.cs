using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Injector : BulletMovement
{
    Animator Anim;
    Enemies Enemy;

    static Injector Inject;

    void Start()
    {
        Anim = GetComponent<Animator>();
        Rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Rig.velocity = (Dir * Speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            transform.parent = collision.collider.transform;
            Enemy = collision.collider.GetComponent<Enemies>();
            GetComponent<Collider2D>().enabled = false;
            if (Enemy.InDazedState)
            {
                Enemy.Injected = true;
            }
            Anim.SetBool("InEnemy", true);
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Cure()
    {
        if (Enemy.CanCure)
        {
            Enemy.Injected = true;
            Movement.Instance.GunInHand.Ammo += 1;
            Enemy.Cure();
        }
        if (!Enemy.CanCure)
        {
            Destroy(gameObject);
        }
    }
}
