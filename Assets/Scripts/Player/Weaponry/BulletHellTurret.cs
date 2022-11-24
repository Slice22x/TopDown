using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public class BulletHellTurret : MonoBehaviour
{
    [SerializeField] private string Identfy;

    public GameObject EnemyBullet;

    [Range(0f, 360f)]
    [SerializeField] private float StartAngle, EndAngle;
    float AngleStep;
    private float StartA, EndA;
    public int FireTurrets = 3;
    public float BulletSpeed = 10f;
    public float Offset;

    public bool Rotate;
    [Range(0f, 10f)]
    public float RotationSpeed;

    public float FireRate = 1f;
    float NextFireTime;

    public float Intensity;

    [SerializeField] bool FireOnStart;

    public enum SpreadTypes
    {
        Normal,
        SinWave,
        InverseSinWave,
        MorphWave,
        MultiWave,
        SpecialRipple
    }

    public SpreadTypes Spread;

    private void Start()
    {
        StartA = StartAngle;
        EndA = EndAngle;
        if (FireOnStart)
            StartCoroutine(Shoot(200, true, 6, 0.2f, 2, 7, SpreadTypes.Normal, 2f));
    }

    void Update()
    {
        if (Rotate)
        {
            StartAngle += RotationSpeed;
            EndAngle += RotationSpeed;
            switch (Spread)
            {
                case SpreadTypes.SinWave:
                    RotationSpeed = FormulaCalculator.Wave(Intensity, Time.time);
                    break;
                case SpreadTypes.InverseSinWave:
                    RotationSpeed = FormulaCalculator.CosWave(Intensity, Time.time);
                    break;
                case SpreadTypes.MorphWave:
                    RotationSpeed = FormulaCalculator.MorphWave(Intensity, Time.time);
                    break;
                case SpreadTypes.MultiWave:
                    RotationSpeed = FormulaCalculator.MultiWave(Intensity, Intensity, Time.time);
                    break;
                case SpreadTypes.SpecialRipple:
                    RotationSpeed = FormulaCalculator.AnimatedRipple(Intensity, Time.time);
                    break;
            }
        }

        else if (!Rotate)
        {
            StartAngle = -transform.eulerAngles.z + StartA;
            EndAngle = -transform.eulerAngles.z + EndA;
        }
    }

    public void Shoot()
    {
        if (Time.time >= NextFireTime)
        {
            float AngleStep = (EndAngle - StartAngle) / FireTurrets;
            float Angle = StartAngle;

            for (int i = 0; i < FireTurrets; i++)
            {
                float DirX = transform.position.x + Sin((Angle * PI) / 180);
                float DirY = transform.position.y + Cos((Angle * PI) / 180);

                Vector3 MoveVector = new Vector3(DirX, DirY, 0f);
                Vector2 Dir = (MoveVector - transform.position).normalized;

                if (!Projectile_Check.Instance.DontSpawn)
                {
                    GameObject NewBullet = Instantiate(EnemyBullet, transform.position + ((Vector3)Dir * Offset), Quaternion.identity);
                    NewBullet.GetComponent<BulletMovement>().Dir = Dir;
                    NewBullet.GetComponent<BulletMovement>().SpeedMultiply = BulletSpeed;
                    NewBullet.GetComponent<BulletMovement>().Speed = BulletSpeed;
                    if (!NewBullet.GetComponent<BulletMovement>().DynamicSpeed)
                    {
                        NewBullet.GetComponent<Rigidbody2D>().AddForce(Dir * BulletSpeed, ForceMode2D.Impulse);
                    }
                }


                Angle += AngleStep;
            }
            NextFireTime = Time.time + 1f / FireRate;
        }
    }

    public IEnumerator Shoot(int Times, float Offset)
    {
        while (Times > 0)
        {
            AngleStep = ((EndAngle - StartAngle) / FireTurrets);
            float Angle = StartAngle;

            for (int i = 0; i < FireTurrets; i++)
            {

                float DirX = transform.position.x + Sin((Angle * PI) / 180);
                float DirY = transform.position.y + Cos((Angle * PI) / 180);

                Vector3 MoveVector = new Vector3(DirX, DirY, 0f);
                Vector2 Dir = (MoveVector - transform.position).normalized;

                if (!Projectile_Check.Instance.DontSpawn)
                {
                    GameObject NewBullet = Instantiate(EnemyBullet, transform.position + ((Vector3)Dir * Offset), Quaternion.identity);
                    NewBullet.GetComponent<BulletMovement>().Dir = Dir;
                    NewBullet.GetComponent<BulletMovement>().SpeedMultiply = BulletSpeed;
                    NewBullet.GetComponent<BulletMovement>().Speed = BulletSpeed;
                    if (!NewBullet.GetComponent<BulletMovement>().DynamicSpeed)
                    {
                        NewBullet.GetComponent<Rigidbody2D>().AddForce(Dir * BulletSpeed, ForceMode2D.Impulse);
                    }
                }
                Angle += AngleStep;
            }
            yield return new WaitForSeconds(FireRate);
            Times -= 1;
        }
        if (Rotate == true)
        {
            this.Rotate = !Rotate;
        }
    }

    public IEnumerator Shoot(int Times, float Offset, System.Action CallWhenDone)
    {

        while (Times > 0)
        {
            AngleStep = ((EndAngle - StartAngle) / FireTurrets);
            float Angle = StartAngle;

            for (int i = 0; i < FireTurrets; i++)
            {

                float DirX = transform.position.x + Sin((Angle * PI) / 180);
                float DirY = transform.position.y + Cos((Angle * PI) / 180);

                Vector3 MoveVector = new Vector3(DirX, DirY, 0f);
                Vector2 Dir = (MoveVector - transform.position).normalized;

                if (!Projectile_Check.Instance.DontSpawn)
                {
                    GameObject NewBullet = Instantiate(EnemyBullet, transform.position + ((Vector3)Dir * Offset), Quaternion.identity);
                    NewBullet.GetComponent<BulletMovement>().Dir = Dir;
                    NewBullet.GetComponent<BulletMovement>().SpeedMultiply = BulletSpeed;
                    NewBullet.GetComponent<BulletMovement>().Speed = BulletSpeed;
                    if (!NewBullet.GetComponent<BulletMovement>().DynamicSpeed)
                    {
                        NewBullet.GetComponent<Rigidbody2D>().AddForce(Dir * BulletSpeed, ForceMode2D.Impulse);
                    }
                }
                Angle += AngleStep;
            }
            yield return new WaitForSeconds(FireRate);
            Times -= 1;
        }
        if (Rotate == true)
        {
            this.Rotate = !Rotate;
        }

        CallWhenDone.Invoke();
    }

    public IEnumerator Shoot(int Times, bool Rotate, int Turrets, float Rate, float RotateSpeed, float Speed, SpreadTypes Spread, float Offset)
    {
        this.Rotate = Rotate;
        FireTurrets = Turrets;
        RotationSpeed = RotateSpeed;
        this.Spread = Spread;
        this.BulletSpeed = Speed;

        while (Times > 0)
        {
            AngleStep = ((EndAngle - StartAngle) / FireTurrets);
            float Angle = StartAngle;

            for (int i = 0; i < FireTurrets; i++)
            {

                float DirX = transform.position.x + Sin((Angle * PI) / 180);
                float DirY = transform.position.y + Cos((Angle * PI) / 180);

                Vector3 MoveVector = new Vector3(DirX, DirY, 0f);
                Vector2 Dir = (MoveVector - transform.position).normalized;

                if (!Projectile_Check.Instance.DontSpawn)
                {
                    GameObject NewBullet = Instantiate(EnemyBullet, transform.position + ((Vector3)Dir * Offset), Quaternion.identity);
                    NewBullet.GetComponent<BulletMovement>().Dir = Dir;
                    NewBullet.GetComponent<BulletMovement>().SpeedMultiply = BulletSpeed;
                    NewBullet.GetComponent<BulletMovement>().Speed = BulletSpeed;
                    if (!NewBullet.GetComponent<BulletMovement>().DynamicSpeed)
                    {
                        NewBullet.GetComponent<Rigidbody2D>().AddForce(Dir * BulletSpeed, ForceMode2D.Impulse);
                    }
                }
                Angle += AngleStep;
            }
            yield return new WaitForSeconds(Rate);
            Times -= 1;
        }
        if (Rotate == true)
        {
            this.Rotate = !Rotate;
        }
    }

    public IEnumerator Shoot(int Times, bool Rotate, int Turrets, float Rate, float RotateSpeed, float Speed, SpreadTypes Spread, System.Action CallWhenDone, float Offset)
    {
        this.Rotate = Rotate;
        FireTurrets = Turrets;
        RotationSpeed = RotateSpeed;
        this.Spread = Spread;
        this.BulletSpeed = Speed;
        this.FireRate = Rate;

        while (Times > 0)
        {
            AngleStep = ((EndAngle - StartAngle) / FireTurrets);
            float Angle = StartAngle;

            for (int i = 0; i < FireTurrets; i++)
            {

                float DirX = transform.position.x + Sin((Angle * PI) / 180);
                float DirY = transform.position.y + Cos((Angle * PI) / 180);

                Vector3 MoveVector = new Vector3(DirX, DirY, 0f);
                Vector2 Dir = (MoveVector - transform.position).normalized;

                if (!Projectile_Check.Instance.DontSpawn)
                {
                    GameObject NewBullet = Instantiate(EnemyBullet, transform.position + ((Vector3)Dir * Offset), Quaternion.identity);
                    NewBullet.GetComponent<BulletMovement>().Dir = Dir;
                    NewBullet.GetComponent<BulletMovement>().SpeedMultiply = BulletSpeed;
                    NewBullet.GetComponent<BulletMovement>().Speed = BulletSpeed;
                    if (!NewBullet.GetComponent<BulletMovement>().DynamicSpeed)
                    {
                        NewBullet.GetComponent<Rigidbody2D>().AddForce(Dir * BulletSpeed, ForceMode2D.Impulse);
                    }
                }

                Angle += AngleStep;
            }
            yield return new WaitForSeconds(Rate);
            Times -= 1;
        }
        if (Rotate == true)
        {
            this.Rotate = !Rotate;
        }

        CallWhenDone.Invoke();
    }
}
