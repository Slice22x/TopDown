using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BossMovement : MonoBehaviour
{
    Rigidbody2D BossRig;
    public bool IsAttacking;

    private AIPath Path;
    AIDestinationSetter Setter;

    public Animator Anim;
    string CurrentState;

    public bool SpeenAttack;
    void Start()
    {
        BossRig = GetComponent<Rigidbody2D>();
        Path = GetComponent<AIPath>();
        Setter = GetComponent<AIDestinationSetter>();
        Setter.target = Movement.Instance.transform;
        SoundManager.Play("Boss1");
    }

    // Update is called once per frame
    void Update()
    {
        if (Path.desiredVelocity.x > 0f)
        {
            transform.localScale = new Vector3(1f, 1f);
        }
        else if (Path.desiredVelocity.x < 0f)
        {
            transform.localScale = new Vector3(-1f, 1f);
        }

        if (IsAttacking)
        {
            Path.maxSpeed = 0f;
        }
        if (!IsAttacking)
        {
            Path.maxSpeed = 2f;
            ChangeState("Walk_Apple");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Bullet"))
        {
            if (SpeenAttack) return;

            // Calculate Angle Between the collision point and the player
            ContactPoint2D contactPoint = collision.GetContact(0);
            Vector2 playerPosition = transform.position;
            Vector2 dir = contactPoint.point - playerPosition;

            // We then get the opposite (-Vector3) and normalize it
            dir = -dir.normalized;

            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GetComponent<Rigidbody2D>().inertia = 0;


            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            GetComponent<Rigidbody2D>().AddForce(dir * 100f, ForceMode2D.Impulse);
        }
        if (collision.collider.CompareTag("Player"))
        {
            if (SpeenAttack)
            {
                Movement.Instance.InAction = true;
                Health.Instance.DealDamage(30, true);
                // Calculate Angle Between the collision point and the player
                Vector2 playerPosition = transform.position;
                Vector2 dir = (Vector2)Movement.Instance.transform.position - playerPosition;

                // We then get the opposite (-Vector3) and normalize it
                dir = dir.normalized;

                Movement.Instance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                Movement.Instance.GetComponent<Rigidbody2D>().inertia = 0;


                // And finally we add force in the direction of dir and multiply it by force. 
                // This will push back the player
                Movement.Instance.GetComponent<Rigidbody2D>().AddForce(dir * 30f, ForceMode2D.Impulse);
                Invoke("EnableControls", 0.6f);
            }
        }
    }

    public void ChangeState(string NextState)
    {
        if (NextState == CurrentState) return;

        Anim.Play(NextState);

        CurrentState = NextState;
    }


    void EnableControls()
    {
        Movement.Instance.InAction = false;
    }
}
