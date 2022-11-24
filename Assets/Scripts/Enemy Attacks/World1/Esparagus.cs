using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Detection))]
[RequireComponent(typeof(Enemies))]
public class Esparagus : MonoBehaviour
{
    public TypeOfEnemy Type;
    private Detection Detect;
    [HideInInspector] public bool StartAttacking;
    public Transform SpinPoint;
    public float WaitTime;
    private Animator Anim;
    private Vector2 NewRotataion;
    private Rigidbody2D EnemRig;
    private Enemies Enem;
    bool CoolingDown;
    void Start()
    {
        Detect = GetComponent<Detection>();
        Anim = GetComponentInChildren<Animator>();
        Enem = GetComponent<Enemies>();
        Detect.DetectionRange = 2;
        NewRotataion.x = 0f;
        NewRotataion.y = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CoolingDown)
        {
            if (!Health.Instance.Invis ||
!Enem.KnifeStuck ||
!Movement.Instance.InAttack ||
!Enem.Elemented)
            {
                Detect.enabled = true;
            }
            else
            {
                Detect.enabled = false;
            }
        }


        StartCoroutine(Attacking());
        if(StartAttacking == true)
        {
            Movement.Instance.transform.position = SpinPoint.position;
        }
        if(StartAttacking == true && Enem.Health <= 0)
        {
            StartAttacking = false;
            Movement.Instance.transform.parent = null;
        }
        if(StartAttacking == false)
        {
            transform.GetChild(0).transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    protected IEnumerator Attacking()
    {
        if (Detect.Detected == true && Enem.Health > ((float)Enem.Health / 4))
        {
            StartAttacking = true;

            if (StartAttacking)
            {
                //Deactivation

                Enem.Attacking = true;
                Enem.ChangeCurrentState(global::Enemies.EState.Attacking);
                Enem.ChangeState(Enem.AttackAnim);
                Movement.Instance.transform.parent = SpinPoint.transform;
                Movement.Instance.transform.position = SpinPoint.position;
                Movement.Instance.GetComponent<Collider2D>().enabled = false;
                if (WeaponManager.Instance.CurrentWeapon != null) WeaponManager.Instance.CurrentWeapon.enabled = false;
                Movement.Instance.enabled = false;
                EnemRig = GetComponent<Rigidbody2D>();
                gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
                EnemRig.velocity = NewRotataion;
                GetComponent<Collider2D>().enabled = false;

                yield return new WaitForSeconds(3f);

                //Reactivation
                Detect.enabled = false;
                Movement.Instance.GetComponent<Health>().DealDamage(Type.Damage,true);
                Movement.Instance.transform.parent = null;
                Vector2 NewPos = new Vector2(gameObject.transform.position.x - 3, Movement.Instance.transform.position.y);
                Movement.Instance.transform.position = NewPos;
                Movement.Instance.GetComponent<Collider2D>().enabled = true;
                if (WeaponManager.Instance.CurrentWeapon != null) WeaponManager.Instance.CurrentWeapon.enabled = true;
                Movement.Instance.enabled = true;
                Enem.ChangeState(Enem.IdleAnim);
                GetComponent<Collider2D>().enabled = true;
                CoolingDown = false;
                Esparagus[] Enemies = FindObjectsOfType<Esparagus>();
                Movement.Instance.transform.rotation = Quaternion.Euler(Vector3.zero);
                Enem.Sq.PlayAnim("Squash");
                CinemachineShake.Instance.ShakeCamera(3f, 0.8f);
                Enem.ChangeCurrentState(global::Enemies.EState.Wandering);
                for (int i = 0; i < Enemies.Length; i++)
                {
                    Enemies[i].Detect.enabled = false;
                    Enemies[i].StartAttacking = false;
                    Enemies[i].CoolingDown = true;
                    Detect.enabled = false;
                    CoolingDown = true;
                }

                yield return new WaitForSeconds(2f);
                for (int i = 0; i < Enemies.Length; i++)
                {
                    Enemies[i].Detect.enabled = true;
                    Enemies[i].Enem.Attacking = false;
                    Enemies[i].CoolingDown = true;
                    CoolingDown = false;
                    Detect.enabled = true;
                    Debug.Log("Enabled");
                }
            }


        }
    }

}
