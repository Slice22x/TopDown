using UnityEngine;
using System.Collections;

public class RockCandy : MonoBehaviour
{
    Detection Detect;
    float AttackTimer;
    public float StaticTimer;
    Rigidbody2D Body;
    Enemies Enemy;
    public float ShakeAmount;
    void Start()
    {
        Detect = GetComponent<Detection>();
        Body = GetComponent<Rigidbody2D>();
        Enemy = GetComponent<Enemies>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Detect.Detected)
        {
            AttackTimer -= Time.deltaTime;
            Vector2 LookDir = (Vector2) Movement.Instance.transform.position - Body.position;
            float Ang = Mathf.Atan2(LookDir.y, LookDir.x) * Mathf.Rad2Deg - 90f;
            Body.rotation = Ang;
            if(AttackTimer <= 0f)
            {
                if (Detect.ObjectsInArea != null)
                {
                    if (Detect.ObjectsInArea.tag == "Player")
                    {
                        LeanTween.move(gameObject, transform.position + new Vector3(transform.up.x + 0.5f,0f), 0.5f).setEase(
                            LeanTweenType.easeInQuart).setOnComplete(() => 
                            Health.Instance.DealDamage(Enemy.EnemyStyler.Damage,true));
                    }
                }
            }
        }
        if (!Detect.Detected)
        {
            AttackTimer = StaticTimer;
            Body.rotation = 0f;
        }
        if (!Enemy.PlayerFound)
        {
            Body.MovePosition(Body.position + (Vector2)Random.insideUnitSphere * ShakeAmount);
        }
    }
}
