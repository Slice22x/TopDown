using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    public Transform Point;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.transform.position = Point.position;
            StartCoroutine(Wait(3f));
        }
    }

    IEnumerator Wait(float WaitTime)
    {
        Point.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(WaitTime);
        Point.GetComponent<BoxCollider2D>().enabled = true;
    }
}
