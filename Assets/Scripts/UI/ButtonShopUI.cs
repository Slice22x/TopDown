using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonShopUI : MonoBehaviour
{
    public ShopUI Cost;
    public Item Selling;
    public int Count;
    public Image SpriteImage;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Count <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SellToPlayer()
    {
        Cost.BuyItem(Selling);
        Count--;
    }
}
