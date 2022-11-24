using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] EnemyToSpawn;
    public int NumberOfEnemies;
    public float RandomTime;
    private int RandomNumber;
    bool Searching;
    public GameObject Spawner;
    // Update is called once per frame
    void Start()
    {


    }

    private void OnEnable()
    {
        StartCoroutine(SpawnEnemy());
    }

    private void Update()
    {
        
    }

    public void SpawnEnemy(string EnemyName, out GameObject EnemyInfo)
    {
        Vector2 RandomPos = new Vector2(Random.Range(-50f, 50f), Random.Range(0f, 100f));
        EnemyInfo = Instantiate(EnemyFinder.Instance.GetEnemyfromName(EnemyName).EnemyObjectt, RandomPos, Quaternion.identity);
    }

    public void SpawnEnemy(string EnemyName, out GameObject EnemyInfo, Vector2 Position)
    {
        EnemyInfo = Instantiate(EnemyFinder.Instance.GetEnemyfromName(EnemyName).EnemyObjectt, Position, Quaternion.identity);
    }

    public void SpawnEnemy(string EnemyName)
    {
        Vector2 RandomPos = new Vector2(Random.Range(-50f, 50f), Random.Range(0f, 100f));
        Instantiate(EnemyFinder.Instance.GetEnemyfromName(EnemyName).EnemyObjectt, RandomPos, Quaternion.identity);
    }

    public IEnumerator SpawnEnemy()
    {
        while (NumberOfEnemies > 0)
        {
            RandomTime = Random.Range(1f, 5f);
            RandomNumber = Random.Range(0, EnemyToSpawn.Length);
            Vector2 RandomPos = new Vector2(Random.Range(-50f, 50f), Random.Range(0f, 100f));
            Instantiate(Spawner, RandomPos, Quaternion.identity).GetComponent<SpawnIcon>().EnemyToSpawn = EnemyToSpawn[RandomNumber];
            NumberOfEnemies--;
            yield return new WaitForSeconds(RandomTime);
        }
    }
}
