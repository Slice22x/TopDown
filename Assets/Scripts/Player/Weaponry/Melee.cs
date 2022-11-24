using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Melee : MonoBehaviour
{
    private float TimeBetweenAttack;
    public float StartBtwAttak;
    public bool AutomaticAttack;
    public Transform AttackPos;
    public float AttackRange;
    public LayerMask WhatIsEnemy;
    public int Damage;
   // private ThowingKnife Knife;
    //float ThrowForece;
    //float ChargeTime;
    const float FixedChargeTime = 1f;
  //  bool HasBeenThrown = false;
    //public GameObject KnifeImage;

  //  public float Speed;
   // public float Distance;
    //public float LerpTime = 0.05f;
   // bool StartMoving;
    //public Detection Detect;

   // public GameObject obj;
    //public bool InKnockback;


    //Vector3 CurrentPos;

    void Start()
    {
        // Damage = Mel.Damage;
        if (SceneType.Instance.ThisScene == SceneType.TypeOfScene.Boss || SceneType.Instance.ThisScene == SceneType.TypeOfScene.Village)
        {
            enabled = false;
            gameObject.SetActive(false);
        }
        //ChargeTime = FixedChargeTime;
        Damage = 5;
      //  Knife = GetComponent<ThowingKnife>();
       // GetComponent<TrailRenderer>().enabled = false;
       // Detect = GetComponent<Detection>();
       // Detect.enabled = false;
      //  GetComponent<Collider2D>().enabled = true;
       // GetComponent<Rigidbody2D>().isKinematic = true;
        AttackPos = Movement.Instance.Hand;
       // InKnockback = false;
       // GetComponent<BoxCollider2D>().enabled = false;
       // Movement.Instance.NoKnife = false;
       // Movement.Instance.KnifeObject = gameObject;
       // Movement.Instance.InAction = false;
    }

    void Update()
    {
        if(TimeBetweenAttack <= 0)
        {
            TimeBetweenAttack = StartBtwAttak;
            if (Mouse.current.rightButton.isPressed)
            {
                Collider2D[] InColl = Physics2D.OverlapCircleAll(AttackPos.position, AttackRange, WhatIsEnemy);
                for (int i = 0; i < InColl.Length; i++)
                {
                    InColl[i].GetComponent<Enemies>().Health -= Damage;
                }
            }
        }
        else
        {
            TimeBetweenAttack -= Time.deltaTime;
        }

        //Move to knife
        //if (StartMoving)
        {
            //LerpTime = 0.037f;
            //Movement.Instance.MoveToKnife(1f,10f,LerpTime, transform, Movement.Instance.transform.up);
           // InKnockback = true;
        }

        //Enables everything when close to player
        //if (Detect.Detected)
        {
           // var Enemy = transform.parent.gameObject;            
           // Enemy.GetComponent<Enemies>().Health -= 1000;
          //  ComboSystem.Instance.AddToCombo(2);
            //TimeManager.Manager.DoSlowmotion();
            //Movement.Instance.NoKnife = false;
           // Movement.Instance.JumpState = true;
           // Movement.Instance.enabled = true;
           // Enemy.GetComponent<Enemies>().KnifeStuck = false;
          //  Enemy.GetComponent<Detection>().enabled = true;
          //  StartMoving = false;
           // Movement.Instance.MoveFromKnide(CurrentPos + new Vector3(5, 5), 0.1f);
           // InKnockback = true;
        }
    }

    //Moves knife to enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (HasBeenThrown)
      //  {
           // if (collision.collider.CompareTag("Enemy") && !Movement.Instance.InAttack)
          //  {
               // GetComponent<Rigidbody2D>().isKinematic = true;
              //  transform.parent = collision.transform;
              //  GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
               // Detect.enabled = true;
              //  GetComponent<TrailRenderer>().enabled = false;
               // CurrentPos = Movement.Instance.transform.position;
          //  }

           //if (!collision.collider.CompareTag("Enemy"))
           // {
              //  HasBeenThrown = false;
             //   Destroy(gameObject);
           // }
      //  }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
       // if (transform.parent.tag == "Enemy")
      //  {
           // GetComponent<Collider2D>().enabled = false;
           // var Enemy = collision.gameObject;
           // Movement.Instance.GetComponentInChildren<Shooting>().enabled = false;
           // Enemy.GetComponent<Enemies>().KnifeStuck = true;
           // Enemy.GetComponent<Detection>().enabled = false;
           // StartMoving = true;
       // }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
       // GetComponent<Collider2D>().enabled = true;
    }

    private void FixedUpdate()
    {
        //if (Mouse.current.rightButton.Was && Knife.CanThrowEnemy)
       // {
          //  Vector2 LookDir = Knife.Enemy.transform.position - transform.position;
           // float Ang = Mathf.Atan2(LookDir.y, LookDir.x) * Mathf.Rad2Deg - 90f;
           // GetComponent<Rigidbody2D>().rotation = Ang;
         //   HasBeenThrown = true;
       // }
       // if (HasBeenThrown)
        //{
            //ChargeTime -= Time.deltaTime;
          //  GetComponent<Rigidbody2D>().isKinematic = false;
          //  Movement.Instance.InAction = true;
      //  }
       // if (ChargeTime <= 0f)
       // {
          //  GetComponent<Rigidbody2D>().AddForce(transform.up * ThrowForece, ForceMode2D.Impulse);            
           // ChargeTime = FixedChargeTime;
          //  HasBeenThrown = false;
        //}
    }

    private void OnDestroy()
    {
       // Instantiate(obj, Movement.Instance.Hand.position, Quaternion.identity, Movement.Instance.transform);
       // Movement.Instance.NoKnife = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPos.position, AttackRange);
    }
}
