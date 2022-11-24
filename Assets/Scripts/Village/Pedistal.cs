using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedistal : MonoBehaviour
{
    public GameObject SellingObject;
    public Item SellinigItem;

    void Start()
    {
        if(transform.childCount > 0)
        {
            SellingObject = transform.GetChild(0).gameObject;
            SellinigItem = SellingObject.GetComponent<Item>();
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = SellinigItem.ItemOfChoice.ArtWork;
        }
        else
        {
            SellingObject = null;
            Debug.LogError("No Item in pedistal fool");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(SellingObject != null && WeaponManager.Instance.CurrentWeapon == null)
            {
                WeaponManager.Instance.LoadWeapon(SellingObject);
                SellingObject.SetActive(false);
                SetItem(null);
            }
        }
    }

    public void GiveItem(NPCAI Customer)
    {
        Instantiate(SellingObject, Customer.Hand);
        SellingObject.SetActive(false);
        SetItem(null);
    }

    public void SetItem(GameObject NewObject)
    {
        SellingObject = NewObject;
    }
}
