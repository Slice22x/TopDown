using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    /*
    public Item Item;
    public int Cost;
    public int PlayerMoney;
    public List<Pedistal> Pedistals = new List<Pedistal>();
    public Pedistal EmptyPedistal;
    Detection Detect;
    public Transform InsidePos;
    public Transform InsidePosForVillage;
    public int Index;
    private QueueHandeler Handle;
    public Transform ExitDoor;
    public float WaitTime = 5f;
    public string Place;
    private NPCAI CurrentlyServing;

    void Start()
    {

        Movement.ThisInstance.InsidePos = InsidePos;
        Handle = GetComponent<QueueHandeler>();
        PlayerMoney = PlayerPrefs.GetInt("CurrentMoney");
        Detect = GetComponent<Detection>();
        foreach (Pedistal item in FindObjectsOfType<Pedistal>())
        {
            if(item.gameObject.scene == gameObject.scene)
            {
                Pedistals.Add(item);
            }
        }
        Movement.ThisInstance.Index = Index;
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        if (SceneManager.GetActiveScene().name == Place)
        {
            Movement.ThisInstance.transform.position = InsidePos.position;
        }
    }

    public void MoveInside()
    {
        List<NPCAI> AIs = new List<NPCAI>();
        AIs = FindObjectsOfType<NPCAI>().ToList<NPCAI>();
        foreach (NPCAI item in AIs)
        {
            if (item.gameObject.scene == gameObject.scene)
            {
                item.transform.position = InsidePos.position;
                AIs.Remove(item);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        var AIs = FindObjectsOfType<NPCAI>();
        foreach (NPCAI item in AIs)
        {
            if (item.gameObject.scene == gameObject.scene)
            {
                item.CurrentShop = this;
            }
        }

        WaitTime -= Time.deltaTime;
        if (WaitTime <= 0)
        {
            ServeCustomer();
            if(CurrentlyServing != null)
            {
                WaitTime = Random.Range(CurrentlyServing.NPC.WaitingRange.x + CurrentlyServing.NPC.BitchyTimeInQueue(0.3f)
    , CurrentlyServing.NPC.WaitingRange.y + CurrentlyServing.NPC.BitchyTimeInQueue(0.3f));
            }
        }


        for (int i = 0; i < Pedistals.Count; i++)
        {
            if(Pedistals[i].SellingObject == null)
            {
                EmptyPedistal = Pedistals[i];
            }
        }
        if(WeaponManager.Instance.CurrentWeapon != null)
        {
            Item = WeaponManager.Instance.CurrentWeapon.GetComponent<Item>();
        }

    }

    public void ServeCustomer()
    {
        QueueingUp Customer = Handle.Queue.GetFirst();


        if (Customer != null)
        {
            CurrentlyServing = Customer.GetComponent<NPCAI>();
            if (Customer.GetComponent<Movement>() != null)
            {
                PlayerSellItem();
            }
            else
            {
                Customer.MoveTo(ExitDoor, Customer.GetComponent<NPCAI>(),NPCAI.State.Wandering,true);
            }
        }
    }

    private void PlayerSellItem()
    {
        SellItem();
        if (Movement.ThisInstance.GetComponentInChildren<Item>() != null) Movement.ThisInstance.GetComponentInChildren<Item>().IsBoughtVar = 1;

        Debug.Log("Bought");
    }

    public void SellItem()
    {
        PlayerMoney -= Item.ItemOfChoice.Cost;
        PlayerPrefs.SetInt("CurrentMoney",PlayerMoney);
        Movement.ThisInstance.InQueue = false;
    }
    public void BuyItem()
    {
        PlayerMoney += Item.ItemOfChoice.Cost;
        PlayerPrefs.SetInt("CurrentMoney", PlayerMoney);
    }
    */
}
