using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Strawberry : MonoBehaviour
{
    public GameObject EnemyBullet;

    [Range(0f, 360f)]
    [SerializeField] private float StartAngle, EndAngle;
    private float StartA, EndA;
    public int FireTimes = 3;
    private Detection Detect;
    public float BulletSpeed = 10f;

    public bool Rotate;
    [Range(0f, 10f)]
    public float RotationSpeed;

    public float FireRate = 1f;
    float NextFireTime;

    private AIPath Path;

    public LayerMask WhatIsPlayer;
    private bool PlayerFound;
    public TypeOfEnemy EnemyStyler;

    private Rigidbody2D Rig;

    public Animator SAnim;
    void Start()
    {
        Detect = GetComponent<Detection>();
        Rig = GetComponent<Rigidbody2D>();
        Path = GetComponent<AIPath>();
        Path.enabled = false;
    }

    void FixedUpdate()
    {
        //My per has some fur on it
        PlayerFound = Physics2D.OverlapCircle(transform.position, EnemyStyler.DetectionRange, WhatIsPlayer);

        if (PlayerFound)
        {
            Path.enabled = true;
        }
        if (!PlayerFound)
        {
            Path.enabled = false;
            if (!GetComponent<Enemies>().InDazedState)
                Wander();
        }
        if (Rotate)
        {
            StartAngle += RotationSpeed;
            EndAngle += RotationSpeed;
        }

        if (!Rotate)
        {
            StartAngle = StartA;
            EndAngle = EndA;
        }
        void Wander()
        {
            if (Time.time >= NextFireTime)
            {
                float AngleStep = (EndAngle - StartAngle) / FireTimes;
                float Angle = StartAngle;
                for (int i = 0; i < FireTimes; i++)
                {
                    float DirX = transform.position.x + Mathf.Sin((Angle * Mathf.PI) / 180);
                    float DirY = transform.position.y + Mathf.Cos((Angle * Mathf.PI) / 180);

                    Vector3 MoveVector = new Vector3(DirX, DirY, 0f);
                    Vector2 Dir = (MoveVector - transform.position).normalized;

                    GameObject NewBullet = Instantiate(EnemyBullet, transform.position, Quaternion.identity);
                    NewBullet.GetComponent<Rigidbody2D>().AddForce(Dir * BulletSpeed, ForceMode2D.Impulse);
                    Angle += AngleStep;
                }
                GetComponentInChildren<Squash>().PlayAnim("Squash");
                NextFireTime = Time.time + 1f / FireRate;
            }
        }


    }
}
