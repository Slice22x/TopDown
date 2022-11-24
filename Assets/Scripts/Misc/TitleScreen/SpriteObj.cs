using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteObj : MonoBehaviour
{
    float RotSpeed;
    public float Speed;

    void Start()
    {
        RotSpeed = Random.Range(2f, 7f);
        Speed = Random.Range(0.01f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += new Vector3(0f, 0f, RotSpeed);
        transform.position += new Vector3(0f, Speed, 0f);
        Destroy(gameObject, 10f);
    }
}
