using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningChecher : MonoBehaviour
{
    public GameObject Minions;
    bool ReadyToSpawn;
    public float WaitTime;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

            WaitTime -= Time.deltaTime;
            if(WaitTime <= 0)
            {
                Instantiate(Minions, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Vector2 RandomPosition = new Vector2(Random.Range(-49f, 49f), Random.Range(-49F, 49f));
            Instantiate(gameObject, RandomPosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
