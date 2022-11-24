using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActualShopUI : MonoBehaviour
{
    public ShopUI ThisUI;
    public List<ThisSellingItem> ItemsToSell = new List<ThisSellingItem>();
    public List<GameObject> IndividualObject = new List<GameObject>();
    public GameObject UI;
    public Transform Holder;
    int Loops = 0;
    public TMP_Text MoneyText;

    public static ActualShopUI Instance;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }

        foreach (ThisSellingItem Sell in ItemsToSell)
        {
            GameObject NewUIElement = Instantiate(UI, Holder, false);
            IndividualObject.Add(NewUIElement);
        }

    }

    public void Leave()
    {
        ThisUI.SellingItems = ItemsToSell;
        ThisUI.ExitLine();
        Destroy(gameObject);
    }

    void Update()
    {

        MoneyText.text = MoneyManager.Instance.CurrentMoney.ToString();
        while (Loops < IndividualObject.Count)
        {
            for (int i = 0; i < IndividualObject.Count; i++)
            {
                IndividualObject[i].name = ItemsToSell[i].ThisItem.Name;
                IndividualObject[i].GetComponentInChildren<ButtonShopUI>().SpriteImage.sprite = ItemsToSell[i].ThisItem.ArtWork;
                IndividualObject[i].GetComponentInChildren<TMP_Text>().text = ItemsToSell[i].ThisItem.Cost.ToString();
                IndividualObject[i].GetComponentInChildren<ButtonShopUI>().Cost = ThisUI;
                IndividualObject[i].GetComponentInChildren<ButtonShopUI>().Selling = ItemsToSell[i].ThisItem.ItemGameObject.GetComponent<Item>();
                IndividualObject[i].GetComponentInChildren<ButtonShopUI>().Count = ItemsToSell[i].Count;
                Loops++;
            }
            
        }
    }
}
