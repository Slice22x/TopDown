using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;
using UnityEngine.UI;
public class Enemies : MonoBehaviour
{
    public enum EState
    {
        Idle,
        Wandering,
        Persuing,
        Attacking,
        Dazed,
        Hurt,
        Dead,
        Cured
    }

    public enum Effect
    {
        None,
        Fire,
        Trapped,
        Ice,
        Poison
    }

    public EState ThisState;

    public LayerMask WhatIsPlayer;
    [HideInInspector] public bool PlayerFound;
    AIPath Path;
    [HideInInspector] public Rigidbody2D Rig;
    Detection Detect;

    private float XVal;
    private float YVal;
    private float TimeBetweenMove = 8f;

    public TypeOfEnemy EnemyStyler;
    public int Health;
    public int Defence;
    public float Speed;
    public int Damage;

    public float KnockbackDist;
    public float KnockbackDur;

    private Transform Target;
    private AIDestinationSetter Destination;

    public bool Attacking = true;

    public GameObject Fuel;
    public GameObject Fuel2;
    public GameObject Fuel3;

    public GameObject Boxes;
    public Animator Anim;

    public string WalkAnim;
    public string HurtAnim;
    public string IdleAnim;
    public string AttackAnim;

    public ParticleSystem Blood;

    string CurrentState;

    bool AlreadySet = false;

    int ThisPriority;

   // [HideInInspector]
    public bool KnifeStuck;
    public GameObject MoneyBags;
 
    [HideInInspector] public float MyMultiplier, MyDamageRate, MyDuration, NextDamageTime, ElementResist;
    [HideInInspector] public int CurrentElementDamage;
    [HideInInspector] public bool Elemented;

    PortalHealth Portal;
    public GameObject FireSystem, PoisonSystem, IceSystem;

    public GameObject HitPointText;
    public GameObject ThisDrop;

    public bool InDazedState, CanCure,Injected;
    public float DazedTimer = 10f;
    float MaxDazeTimer;

    GameObject SliderCanvas;
    public bool DontSearch;

    [HideInInspector]public Squash Sq;

    public Effect CurrectEffect;

    void Start()
    {
        Path = GetComponent<AIPath>();
        Path.enabled = false;
        Speed = Mathf.RoundToInt(Movement.Instance.Info.RepModifiyer(EnemyStyler.Speed, !Movement.Instance.Info.VillageSide()));
        Health = Mathf.RoundToInt(Movement.Instance.Info.RepModifiyer(EnemyStyler.Health, !Movement.Instance.Info.VillageSide()));
        Defence = Mathf.RoundToInt(Movement.Instance.Info.RepModifiyer(EnemyStyler.Defence, !Movement.Instance.Info.VillageSide()));
        Damage = Mathf.RoundToInt(Movement.Instance.Info.RepModifiyer(EnemyStyler.Speed, !Movement.Instance.Info.VillageSide()));
        ElementResist = Mathf.RoundToInt(Movement.Instance.Info.RepModifiyer(EnemyStyler.ElementResistance, !Movement.Instance.Info.VillageSide()));
        Detect = GetComponent<Detection>();
        Destination = GetComponent<AIDestinationSetter>();
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        Destination.target = Target;
        Portal = GameObject.FindGameObjectWithTag("Portal").GetComponent<PortalHealth>();
        Portal.NumberOfEnemies++;
        AmountOfEnemies.Instance.Enemies.Add(gameObject);
        SliderCanvas = transform.Find("SliderCanvas").gameObject;
        SliderCanvas.SetActive(false);
        MaxDazeTimer = DazedTimer;
        Sq = GetComponentInChildren<Squash>();
    }

    private void Update()
    {
        #region ELEMENT DAMAGE

        if(CurrectEffect != Effect.None)
        {
            MyDuration -= Time.deltaTime;
            if (Time.time >= NextDamageTime)
            {
                var DamageF = ((float)CurrentElementDamage * MyMultiplier) / (ElementResist / 100);
                Health -= Mathf.RoundToInt(DamageF);
                GameObject TextHit = Instantiate(HitPointText, transform.position, Quaternion.identity);
                TextHit.GetComponentInChildren<TMP_Text>().text = Mathf.RoundToInt(DamageF).ToString();
                NextDamageTime = Time.time + 1f / MyDamageRate;
                FireSystem.SetActive(true);
                if (Health - DamageF < 0)
                {
                    MyDuration = 0f;
                }
            }
        }
        else
        {
            FireSystem.SetActive(false);
            PoisonSystem.SetActive(false);
            IceSystem.SetActive(false);
            if (EnemyStyler.EnemyTpye != TypeOfEnemy.EnemyStyle.Smarties)
            {
                GetComponentInChildren<SpriteRenderer>().color = Color.white;
            }
        }
        if(MyDuration >= 0f)
        {
            switch (CurrectEffect)
            {
                case Effect.Fire:
                    if (EnemyStyler.EnemyTpye != TypeOfEnemy.EnemyStyle.Smarties)
                    {
                        GetComponentInChildren<SpriteRenderer>().color = Color.red;
                    }
                    break;
                case Effect.Poison:
                    if (EnemyStyler.EnemyTpye != TypeOfEnemy.EnemyStyle.Smarties)
                    {
                        GetComponentInChildren<SpriteRenderer>().color = Color.blue;
                    }
                    Path.maxSpeed = Speed - 3;
                    break;
                case Effect.Ice:
                    if (EnemyStyler.EnemyTpye != TypeOfEnemy.EnemyStyle.Smarties)
                    {
                        GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
                    }
                    Path.maxSpeed = Speed - 3;
                    break;
                case Effect.Trapped:
                    if (EnemyStyler.EnemyTpye != TypeOfEnemy.EnemyStyle.Smarties)
                    {
                        GetComponentInChildren<SpriteRenderer>().color = Color.grey;
                    }
                    Path.maxSpeed = Speed - 3;
                    break;
            }
        }
        if(MyDuration <= 0f)
        {
            CurrectEffect = Effect.None;
        }
        #endregion

        switch (ThisState)
        {
            case EState.Wandering:
                UpdateWanderState();
                break;
            case EState.Idle:
                UpdateIdleState();
                break;
            case EState.Persuing:
                UpdatePersueState();
                break;
            case EState.Dazed:
                UpdateDazedState();
                break;
            case EState.Attacking:
                UpdateAttackState();
                break;
        }

        if (Health <= 0)
        {
            Portal.NumberOfEnemies--;
            AmountOfEnemies.Instance.Enemies.Remove(gameObject);
            ChangeCurrentState(EState.Dazed);
        }
    }

    protected void FixedUpdate()
    {
        if (!DontSearch)
            PlayerFound = Physics2D.OverlapCircle(transform.position, EnemyStyler.DetectionRange, WhatIsPlayer);
        if (DontSearch)
            PlayerFound = false;

        if (Path.desiredVelocity.x > 0f)
        {
            transform.localScale = new Vector3(1f, 1f);
        }
        else if (Path.desiredVelocity.x < 0f)
        {
            transform.localScale = new Vector3(-1f, 1f);
        }

        if (!InDazedState)
        {
            if (!Attacking)
            {
                if (PlayerFound)
                {
                    ChangeCurrentState(EState.Persuing);
                }
                if (!PlayerFound)
                {
                    ChangeCurrentState(EState.Wandering);
                }
            }
        }

        if (Attacking)
        {
            ChangeCurrentState(EState.Attacking);
        }

        if (InDazedState)
        {
            ChangeCurrentState(EState.Dazed);
        }

    }

    #region \\========================= IDLE STATE =========================//
    void EnterIdleState()
    {
        Path.enabled = false;
        if (InDazedState)
            return;

        if (EnemyStyler.EnemyTpye == TypeOfEnemy.EnemyStyle.Esparagus)
        {
            if (!GetComponent<Esparagus>().StartAttacking && AlreadySet == false)
            {
                transform.GetChild(0).transform.rotation = Quaternion.Euler(Vector3.zero);
                AlreadySet = true;
                ChangeState(IdleAnim);
            }
        }
        else if (AlreadySet == false)
        {
            transform.GetChild(0).transform.rotation = Quaternion.Euler(Vector3.zero);
            AlreadySet = true;
            ChangeState(IdleAnim);
        }

    }
    void UpdateIdleState()
    {
        ChangeCurrentState(EState.Wandering);
    }
    void ExitIdleState()
    {

    }
    #endregion

    #region \\========================= WANDER STATE========================= //

    GameObject WTarget;
    GameObject NTarget;
    void EnterWanderState()
    {
        WTarget = (GameObject)Resources.Load("WanderTarget", typeof(GameObject));
        Path.enabled = false;
        if (InDazedState)
            return;

        if (EnemyStyler.EnemyTpye == TypeOfEnemy.EnemyStyle.Esparagus)
        {
            if (!GetComponent<Esparagus>().StartAttacking && AlreadySet == false)
            {
                transform.GetChild(0).transform.rotation = Quaternion.Euler(Vector3.zero);
                AlreadySet = true;
            }
        }
        else if (AlreadySet == false)
        {
            transform.GetChild(0).transform.rotation = Quaternion.Euler(Vector3.zero);
            AlreadySet = true;
        }
    }
    void UpdateWanderState()
    {
        if (Attacking) return;

        TimeBetweenMove -= Time.deltaTime;

        if(TimeBetweenMove > 0f)
        {
            ChangeState(IdleAnim);
        }

        if (TimeBetweenMove <= 0f)
        {
            Path.enabled = true;
            ChangeState(WalkAnim);
            XVal = Random.Range(-1f, 1f);
            YVal = Random.Range(-1f, 1f);
            Vector2 Direction = new Vector2(XVal * EnemyStyler.WanderSpeed, YVal * EnemyStyler.WanderSpeed);
            NTarget = Instantiate(WTarget, (Vector2)transform.position + Direction, Quaternion.identity);
            SetTarget(NTarget.transform, 3, true);
            TimeBetweenMove = Random.Range(8f, 15f);
        }
        if(NTarget != null)
        {
            if (Vector2.Distance(transform.position, NTarget.transform.position) < 0.27f)
            {
                ChangeState(IdleAnim);
                Destroy(NTarget);
            }
        }

    }
    void ExitWanderState()
    {

    }

    #endregion

    #region \\========================= PERSUE STATE =========================//
    void EnterPersueState()
    {
        SetTarget(Movement.Instance.transform, 9, false);
        Path.enabled = true;
    }
    void UpdatePersueState()
    {
        ChangeState(WalkAnim);
    }
    void ExitPersueState()
    {
        Vector2 Stop = new Vector2(0f, 0f);
        Rig.velocity = Stop;
    }

    #endregion

    #region \\========================= DAZED STATE =========================//

    Slider Slide;

    void EnterDazedState()
    {
        Path.enabled = false;
        Detect.enabled = false;
        Rig.isKinematic = true;
        ChangeState(IdleAnim);
        Vector2 Stop = new Vector2(0f, 0f);
        Rig.velocity = Stop;
        SliderCanvas.SetActive(true);
        Slide = SliderCanvas.GetComponentInChildren<Slider>();
        Slide.maxValue = MaxDazeTimer;
        InDazedState = true;
        gameObject.layer = 22;
    }
    void UpdateDazedState()
    {
        if (!Injected)
        {
            DazedTimer -= Time.smoothDeltaTime;
            Slide.value = DazedTimer;
        }
        switch (DazedTimer)
        {
            case float _ when DazedTimer > 1f:
                CanCure = true;
                break;
            case float _ when DazedTimer <= 0f:
                ChangeCurrentState(EState.Dead);
                break;
        }
    }
    void ExitDazedState()
    {
        if (!Injected)
        {
            PortalInfo.Instance.ThisLevel.EnemiesDead++;
            DropItems();
            Movement.Instance.Info.Reputation += 3;
        }
    }

    #endregion

    #region \\========================= DEAD STATE =========================//

    GameObject DeadSystem;

    void EnterDeadState()
    {
        DeadSystem = (GameObject)Resources.Load("Blood Explode", typeof(GameObject));
        GameObject NewSus = Instantiate(DeadSystem, transform.position, Quaternion.identity);
        Destroy(NewSus, 1.3f);
        Destroy(gameObject);
    }

    #endregion

    #region \\========================= HURT STATE =========================//
    void EnterHurtState()
    {
        ChangeState(HurtAnim);
        Sq.PlayAnim("Stretch");
        SoundManager.Play("Hurt");
    }
    void UpdateHurtState()
    {
        ChangeState(WalkAnim);
    }
    void ExitHurtState()
    {
        
    }

    #endregion

    #region \\========================= CURED STATE =========================//
    void EnterCureState()
    {
        ChangeState(HurtAnim);
        Sq.PlayAnim("Stretch");
        SoundManager.Play("Hurt");
        Cure();
    }

    #endregion

    #region \\========================= ATTACKING STATE =========================//
    void EnterAttackState()
    {
        ChangeState(AttackAnim);
    }
    void UpdateAttackState()
    {
        ChangeState(AttackAnim);
    }
    void ExitAttackState()
    {

    }

    #endregion

    #region \\========================= OTHER FUNCTIONS =========================//

    #region DROP ITEMS

    void DropItems()
    {
        if (Random.value > 0.5)
        {
            Instantiate(ThisDrop, transform.position, Quaternion.identity);
        }
        if (Random.value > 0.8)
        {
            Instantiate(Fuel, transform.position, Quaternion.identity);
        }
        if (Random.value > 0.9)
        {
            Instantiate(Fuel2, transform.position, Quaternion.identity);
        }
        if (Random.value > 0.95)
        {
            Instantiate(Fuel3, transform.position, Quaternion.identity);
        }
        if (Random.value > 0.7)
        {
            Instantiate(Boxes, transform.position, Quaternion.identity);
        }
        if (Random.value - (ComboSystem.Instance.Combo / 10) > 0.8)
        {
            Instantiate(Fuel, transform.position, Quaternion.identity);
        }
        if (Random.value - (ComboSystem.Instance.Combo / 10) > 0.9)
        {
            Instantiate(Fuel2, transform.position, Quaternion.identity);
        }
        if (Random.value - (ComboSystem.Instance.Combo / 10) > 0.95)
        {
            Instantiate(Fuel3, transform.position, Quaternion.identity);
        }
        if (Random.value - (ComboSystem.Instance.Combo / 10) > 0.7)
        {
            Instantiate(Boxes, transform.position, Quaternion.identity);
        }

        ComboSystem.AddToCombo();
        Vector3 Vex = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0);
        Instantiate(MoneyBags, transform.position + Vex, Quaternion.identity);
    }
    #endregion

    public void ChangeCurrentState(EState State)
    {
        if(ThisState != State)
        {
            switch (ThisState)
            {
                case EState.Wandering:
                    ExitWanderState();
                    break;
                case EState.Idle:
                    ExitIdleState();
                    break;
                case EState.Persuing:
                    ExitPersueState();
                    break;
                case EState.Dazed:
                    ExitDazedState();
                    break;
                case EState.Attacking:
                    ExitAttackState();
                    break;
            }

            switch (State)
            {
                case EState.Wandering:
                    EnterWanderState();
                    break;
                case EState.Idle:
                    EnterIdleState();
                    break;
                case EState.Persuing:
                    if (!DontSearch)
                        EnterPersueState();
                    break;
                case EState.Dazed:
                    EnterDazedState();
                    break;
                case EState.Dead:
                    EnterDeadState();
                    break;
                case EState.Attacking:
                    EnterAttackState();
                    break;
            }
            ThisState = State;
        }

    }

    public void SetTarget(Transform Target, int Priority, bool Bypass)
    {
        if(ThisPriority < Priority || Bypass)
        {
            Destination.target = Target;
            ThisPriority = Priority;
        }
        else if(ThisPriority == Priority && Bypass)
        {
            Destination.target = Target;
            ThisPriority = Priority;
        }
    }

    void ReturnControl()
    {
        DontSearch = false;
        Path.enabled = true;
        Destination.enabled = true;
        ChangeCurrentState(EState.Idle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            ChangeCurrentState(EState.Hurt);

            DontSearch = true;
            Path.enabled = false;
            Destination.enabled = false;
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
            GetComponent<Rigidbody2D>().AddForce(dir * KnockbackDist, ForceMode2D.Impulse);
            Invoke("ReturnControl", KnockbackDur);

            Vector2 Pos = (Vector2)transform.position - collision.contacts[0].point;
            Vector2 LookDir = Pos - (Vector2)transform.position;
            float Ang = Mathf.Atan2(LookDir.y, LookDir.x) * Mathf.Rad2Deg + 90f;
            Blood.transform.position = (Vector2)transform.position - Pos;
            Blood.transform.rotation = Quaternion.Euler(0f, 0f, Ang);
            Blood.Play();
        }

        if (collision.collider.CompareTag("Player"))
        {
            if (InDazedState)
                return;
            var PlayerHealth = collision.collider.gameObject.GetComponent<Health>();
            if(PlayerHealth.Invis == false)
            {
                PlayerHealth.DealDamage(Damage,true);
            }
        }
    }

    public void Cure()
    {
        InDazedState = false;
        DropItems();
        PortalInfo.Instance.ThisLevel.EnemiesCured++;
        Movement.Instance.Info.Reputation -= 6;
        Destroy(gameObject);
    }

    public void ChangeState(string NextState)
    {
        if (NextState == CurrentState) return;

        Anim.Play(NextState);

        CurrentState = NextState;
    }
    #endregion
}
