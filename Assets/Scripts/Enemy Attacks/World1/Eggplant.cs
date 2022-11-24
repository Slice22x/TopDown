using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eggplant : MonoBehaviour
{
    private Detection Detect;
    public GameObject Explosion;
    private Animator Anim;
    void Start()
    {
        Detect = GetComponent<Detection>();
        Anim = GetComponentInChildren<Animator>();
        Detect.DetectionRange = 3;
    }

    private void Update()
    {
        if (Detect.Detected)
        {
            Anim.SetBool("Exploding", true);
            Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    

}
