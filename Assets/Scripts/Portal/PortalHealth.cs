using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.Rendering;

public class PortalHealth : MonoBehaviour
{
    public float Health = 1000;
    private float MaxHealth = 1000;
    private int MaxHealthInt;
    private int HealthInt;
    public float HealthPercent;

    public bool IsOpenForAttack;
    public EnemySpawner Spawner;

    public bool Phase1 = true;
    public bool Phase2 = false;
    public bool Phase3 = false;
    public bool Phase4 = false;

    public int Enemy1Count, Enemy2Count, Enemy3Count, Enemy4Count;

    public GameObject Object;
    public GameObject IconObject;
    public HealthBar Bar;

    public GameObject[] Boxes;
    private int RandomBox;
    private int Times;

    [HideInInspector]
    public int NumberOfEnemies;

    public GameObject WinIndicator;
    public TMP_Text MoneyText;

    public static PortalHealth Instance;

    public GameObject Explosion;

    public PlayableDirector Cutscene;

    public delegate void OnDeath();
    public static event OnDeath PortalDeath;

    bool Died;

    public RenderPipelineAsset Asset;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Spawner = GetComponent<EnemySpawner>();
    }

    void Start()
    {
        Spawner = GetComponent<EnemySpawner>();
        MaxHealthInt = (int) MaxHealth;
        Bar.SetMaxValue(MaxHealthInt);
        Times = 3;
    }

    public void StartGame()
    {
        Spawner = GetComponent<EnemySpawner>();
        MaxHealthInt = (int)MaxHealth;
        Bar.SetMaxValue(MaxHealthInt);
        Times = 3;
        NumberOfEnemies = Enemy1Count;
    }

    public void EndGame()
    {
        IsOpenForAttack = true;
        FindObjectOfType<AmountOfEnemies>().DestroyAllEnemies();
    }
    
    public void CallRipple()
    {
        GraphicsSettings.renderPipelineAsset = Asset;
    }

    public void StartDeathScene(int number)
    {
        StartCoroutine(DeathScene(number));
    }

    public IEnumerator DeathScene(int Number)
    {
        float Numb = Number;
        Movement.Instance.InAction = true;
        WeaponManager.Instance.DropWeapon();
        while(Numb > 0)
        {
            var Rand = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            ExplosionScript.CreateExplosion(Explosion, transform.position + Rand, 1f, 0, true, global::Health.EffectType.None);
            yield return new WaitForSeconds(Random.Range(0.5f, 0.75f));
            Numb--;

            if(Numb <= 0)
            {
                yield return null ;
            }

        }
        yield return new WaitForSeconds(0.5f);
        ExplosionScript.CreateBigExplosion(Explosion, transform.position , 3f, 0, true, global::Health.EffectType.None);
        FindObjectOfType<Exit>().LoadScene("WinScreen");
        Destroy(gameObject);
    }

    void Update()
    {

        HealthPercent = ((Health / MaxHealth) * 100);

        HealthInt = (int)Health;

        string Vals = "Portal:" + HealthInt.ToString() + "/" + MaxHealthInt.ToString();
        Bar.SetText(Vals);
        Bar.SetValue(HealthInt);

        if (!IsOpenForAttack)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            Object.SetActive(false);
            IconObject.SetActive(false);
            Times = 3;

        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = true;
            Object.SetActive(true);
            IconObject.SetActive(true);
            while (Times > 0)
            {
                RandomBox = Random.Range(0, Boxes.Length);
                Vector2 Rand = new Vector2(transform.position.x + Random.Range(-5f, 5f), transform.position.y + Random.Range(-5f, 5f));
                Instantiate(Boxes[RandomBox], Rand, Quaternion.identity);
                Times--;
            }


        }

        if (Spawner.NumberOfEnemies > 0)
        {
            IsOpenForAttack = false;
        }

        if(Spawner.NumberOfEnemies == 0 && AmountOfEnemies.Instance.Enemies.Count == 0)
        {
            Spawner.enabled = false;
            IsOpenForAttack = true;
            
        }

        switch (HealthPercent)
        {
            case float n when HealthPercent == 100 && Phase1 == false:
                Spawner.NumberOfEnemies = Enemy1Count;
                Phase1 = true;
                break;
            case float n when HealthPercent <= 75 && HealthPercent > 50 && Phase2 == false:
                Spawner.enabled = false;
                Phase1 = false;
                Spawner.NumberOfEnemies = Enemy2Count;
                Phase2 = true;
                break;
            case float n when HealthPercent <= 50 && HealthPercent > 25 && Phase3 == false:
                Spawner.enabled = false;
                Phase2 = false;
                Spawner.NumberOfEnemies = Enemy3Count;
                Phase3 = true;
                break;
            case float n when HealthPercent <= 25 && Phase4 == false:
                Spawner.enabled = false;
                Phase3 = false;
                Spawner.NumberOfEnemies = Enemy4Count;
                Phase4 = true;
                break;

        }
        Spawner.enabled = true;
        if (Health <= 0 )
        {
            if (!Died)
            {
                Cutscene.Play();
                if (PortalDeath != null)
                    PortalDeath.Invoke();
                Died = true;
            }

        }
    }
}
