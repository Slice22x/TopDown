using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnIcon : MonoBehaviour
{
    [SerializeField]SpriteRenderer Icon;
    [SerializeField]TypeOfEnemy Enemy;
    public GameObject EnemyToSpawn;
    public TypeOfEnemy DefaultBanana;

    public void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Obstacle")
        {
            transform.position = new Vector2(Random.Range(-50f, 50f), Random.Range(0f, 100f));
        }
    }

    public void ChangeColour()
    {
        Icon = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if(EnemyToSpawn.GetComponent<Enemies>() != null)
        {
            Enemy = EnemyToSpawn.GetComponent<Enemies>().EnemyStyler;
        }
        else
        {
            Enemy = DefaultBanana;
        }
        GetComponent<SpriteRenderer>().color = Enemy.EnemyColour;
        Icon.sprite = Enemy.EnemySprite;
        Icon.color = new Color(255f, 255f, 255f, 255f);
    }

    public void SpawnEnemy()
    {
        Instantiate(EnemyToSpawn, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
