using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    public WeaponManager Manager;
    public bool Droppeed;
    private float WaitTime = 3f;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Manager = WeaponManager.Instance;
        if (Droppeed)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Shooting>().IsOnPlayer = false;
            WaitTime -= Time.deltaTime;
            if (WaitTime <= 0)
            {
                GetComponent<BoxCollider2D>().enabled = true;
                WaitTime = 3f;
                Droppeed = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(Manager.CurrentWeapon == null)
            {
                if(!Manager.NoPickup)
                    Manager.LoadWeapon(gameObject);
            }

        }
    }
}
