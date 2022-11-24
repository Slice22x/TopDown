using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggers : MonoBehaviour
{
    public delegate void BeenTriggered(string Name);
    public static event BeenTriggered Triggered;

    [SerializeField] string SpecTag; 
    [SerializeField] string NameOfTrigger;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == SpecTag)
        {
            if (Triggered != null)
                Triggered.Invoke(NameOfTrigger);
        }
    }
}
