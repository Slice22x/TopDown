using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Apple : MonoBehaviour
{

    public GameObject Warning;
    public float MaxWait, MinWait;
    private float WaitTime;
    public float TimeToAttack;
    int Numb;
    public Detection AttackArea;
    public BulletHellTurret Turret1,Turret2, Turret3, Turret4, Turret5, Turret6;
    public int AttackNumber;
    BossMovement BossM;
    BossHealth BossH;

    public bool DroppedItems;

    public GameObject Fuel, Fuel2, Fuel3, Boxes;

    public bool InDenial;

    public bool Phase2;

    void Start()
    {
        AttackNumber = Random.Range(0, 4);
        TimeToAttack = Random.Range(MinWait, MaxWait);
        Numb = Random.Range(5, 10);
        BossM = GetComponentInParent<BossMovement>();
        BossH = GetComponentInParent<BossHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.leftAltKey.wasPressedThisFrame)
        {
            if (SceneType.Instance.ThisScene == SceneType.TypeOfScene.Boss)
            {
                PauseScreen.Instance.gameObject.SetActive(!PauseScreen.Instance.gameObject.activeSelf);
            }

        }

        if (!BossM.IsAttacking)
            TimeToAttack -= Time.deltaTime;
        if (TimeToAttack <= 0f && !BossM.IsAttacking)
        {
            if(BossH.HealthPercent > 50f)
            {
                switch (AttackNumber)
                {
                    case 0:
                        BossM.ChangeState("Boss_Speen");
                        BossM.SpeenAttack = true;
                        StartCoroutine(SpawnApples());
                        break;
                    case 1:
                        BossM.IsAttacking = true;
                        BossM.ChangeState("Slash");
                        break;
                    case 2:
                        BossM.IsAttacking = true;
                        BossM.ChangeState("GroundPound");
                        break;
                    case 3:
                        BossM.IsAttacking = true;
                        BossM.ChangeState("Boss_Speen");
                        BossM.SpeenAttack = true;
                        StartCoroutine(Turret4.Shoot(20, true, 4, 0.6f, 1f, 5f, BulletHellTurret.SpreadTypes.Normal, ReEnable, 2.5f));
                        StartCoroutine(Turret3.Shoot(1, true, 6, 3, 5f, 10f, BulletHellTurret.SpreadTypes.Normal, 3f));
                        break;
                }
            }
            if(BossH.HealthPercent < 50f)
            {
                switch (AttackNumber)
                {
                    case 0:
                        BossM.ChangeState("Boss_Speen");
                        StartCoroutine(Turret4.Shoot(45, true, 5, 0.2f, 1f, 5f, BulletHellTurret.SpreadTypes.Normal, 2.5f));
                        StartCoroutine(SpawnApples());
                        break;
                    case 1:
                        BossM.IsAttacking = true;
                        BossM.ChangeState("Slash_Angry");
                        break;
                    case 2:
                        BossM.IsAttacking = true;
                        BossM.ChangeState("GroundPound_Angry");
                        break;
                    case 3:
                        BossM.IsAttacking = true;
                        BossM.ChangeState("Boss_Speen");
                        StartCoroutine(Turret4.Shoot(90, true, 5, 0.2f, 1f, 5f, BulletHellTurret.SpreadTypes.Normal, ReEnable, 2.5f));
                        StartCoroutine(Turret3.Shoot(5, true, 6, 3, 0.5f, 10f, BulletHellTurret.SpreadTypes.Normal, 3.5f));
                        break;
                }
            }
            
            if(BossH.HealthPercent < 50f && !DroppedItems)
            {
                StopAllCoroutines();
                ReEnable();
                for (int i = 0; i < 3; i++)
                {
                    DropItems();
                }

                DroppedItems = true;
            }

        }
        if(TimeToAttack < -10f)
        {
            ReEnable();
        }
    }

    public void Speen()
    {
        BossM.ChangeState("Speen");
    }

    public void Juice(string Anim)
    {
        GetComponentInParent<Squash>().PlayAnim(Anim);
    }

    IEnumerator SpawnApples()
    {
        while(Numb > 0)
        {
            BossM.IsAttacking = true;
            TimeToAttack = 5f;
            Vector2 RandomPosition = (Vector2)Movement.Instance.transform.position + new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            Instantiate(Warning, RandomPosition, Quaternion.identity);
            Numb--;
            yield return new WaitForSeconds(Random.Range(1.5f, 2f));
        }
        ReEnable();
    }

    public void RotateAt()
    {
        Vector2 LookDir = Movement.Instance.transform.position - transform.position;
        float Ang = Mathf.Atan2(LookDir.y, LookDir.x) * Mathf.Rad2Deg + 90f;
        GetComponentInParent<Rigidbody2D>().rotation = Ang;
    }

    public void SwingAtPlayer()
    {
        AttackArea.enabled = true;
        if (AttackArea.Detected)
        {
            Health.Instance.DealDamage(50,true);
        }
        StartCoroutine(Turret2.Shoot(1, false, 15,0.1f,0f,6f, BulletHellTurret.SpreadTypes.Normal,1f));
        AttackArea.enabled = false;
    }

    public void ReEnable()
    {
        if (BossM.IsAttacking)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
            transform.parent.rotation = Quaternion.Euler(Vector3.zero);
            BossM.IsAttacking = false;
            BossM.SpeenAttack = false;
            StopCoroutine(SpawnApples());
            Juice("New Animation");
            Numb = Random.Range(5, 10);
            AttackNumber = Random.Range(0, 4);
            TimeToAttack = Random.Range(MinWait, MaxWait);
        }

    }

    void DropItems()
    {
        Instantiate(Boxes, Movement.Instance.transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), Quaternion.identity);
        Instantiate(Boxes, Movement.Instance.transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), Quaternion.identity);
        Instantiate(Boxes, Movement.Instance.transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), Quaternion.identity);

        Instantiate(Fuel3, Movement.Instance.transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), Quaternion.identity);
        Instantiate(Fuel3, Movement.Instance.transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), Quaternion.identity);
        Instantiate(Fuel3, Movement.Instance.transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), Quaternion.identity);
    }

    public void DenialDeath()
    {
        ReEnable();
        StopAllCoroutines();
        DropItems();
        BossM.IsAttacking = true;
        TimeToAttack = 300f;
        WeaponManager.Instance.NoPickup = true;
        BossM.ChangeState("Apple_Denial");
        BossM.transform.position = new Vector3(0f, 50f);
        StartCoroutine(Turret6.Shoot(90,3));
        StartCoroutine(Turret3.Shoot(20, true, 1, 0.6f, 1f, 5f, BulletHellTurret.SpreadTypes.Normal, 2.5f));
        StartCoroutine(Turret5.Shoot(250,2.5f,Death));
        InDenial = true;
    }

    public void Death()
    {
        BossM.ChangeState("Apple_Death");
        BossH.CallDie();
    }

    public void GroundStrike()
    {
        StartCoroutine(Turret1.Shoot(3,true,10,0.2f,3f,6f, BulletHellTurret.SpreadTypes.Normal, 1f));
    }
}
