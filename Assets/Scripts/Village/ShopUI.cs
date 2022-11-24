using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ThisSellingItem
{
    public ItemScriptableObject ThisItem;
    public int Count;

    public ThisSellingItem(ItemScriptableObject Adder, int Count)
    {
        this.ThisItem = Adder;
        this.Count = Count;
    }
}

public class ShopUI : MonoBehaviour
{
    private Detection Detect;
    public QueueHandeler Handle;
    public float WaitBeforeNextCustomer;
    private const float FixedWaitingTime = 10f;
    public NPCAI CurrentlyServing;
    public GameObject CanvasObject;
    public Transform ShopExit;
    public List<ThisSellingItem> SellingItems = new List<ThisSellingItem>();
    public GameObject ShopObject;
    GameObject InstantiatedObject;

    void Start()
    {
        Detect = GetComponent<Detection>();
    }

    public void ExitLine()
    {
        CanvasObject.GetComponentInChildren<ActualShopUI>().enabled = false;
        CanvasObject.SetActive(false);
        Destroy(InstantiatedObject);
        InstantiatedObject = null;
        Movement.Instance.LeaveQueue(Handle);
        Movement.Instance.enabled = true;
    }

    void Update()
    {
        if(Handle.Queue.CustomerList.Count > 0)
        {
            WaitBeforeNextCustomer -= Time.deltaTime;
        }
        if (WaitBeforeNextCustomer <= 0)
        {
            ServeCustomer();
        }

        if (Detect.Detected)
        {
            if (Detect.ObjectsInArea.GetComponent<NPCAI>() != null)
            {
                if (!Detect.ObjectsInArea.GetComponent<NPCAI>().InQueue)
                {
                    if (Detect.ObjectsInArea.GetComponent<NPCAI>().ThisState == NPCAI.State.Wandering ||
  Detect.ObjectsInArea.GetComponent<NPCAI>().ThisState == NPCAI.State.HomeLonging || 
  Detect.ObjectsInArea.GetComponent<NPCAI>().ThisState == NPCAI.State.Speaking)
                        return;
                    else if (Detect.ObjectsInArea.GetComponent<NPCAI>() != null)
                    {
                        Detect.ObjectsInArea.GetComponent<NPCAI>().SwitchState(NPCAI.State.Queueing);
                        Detect.ObjectsInArea.GetComponent<NPCAI>().InShop = true;
                    }
                }
            }
            else if(Detect.ObjectsInArea.GetComponent<Movement>() != null)
            {
                Movement.Instance.InShopArea = true;
                Movement.Instance.ActionPrompt = true;
                if (Movement.Instance.Interact.triggered)
                {
                    Detect.ObjectsInArea.GetComponent<QueueingUp>().QueueUp(GetComponent<QueueHandeler>());
                }
            }
        }
        if (!Detect.Detected)
        {
            Movement.Instance.InShopArea = false;
        }

    }

    public void ServeCustomer()
    {
        QueueingUp Customer = Handle.Queue.GetFirst();

        if (Customer == null)
        {
            WaitBeforeNextCustomer = FixedWaitingTime;
            return;
        }


        if (Customer != null)
        {
            CurrentlyServing = Customer.GetComponent<NPCAI>();
            if (Customer.GetComponent<Movement>() != null)
            {
                PlayerSellItem();
            }
            else
            {
                Customer.MoveToDo(ShopExit.position, () => 
                {
                    Customer.GetComponent<NPCAI>().InQueue = false;
                    Customer.GetComponent<NPCAI>().InShop = false;
                    Customer.GetComponent<NPCAI>().SwitchState(NPCAI.State.HomeLonging);
                });
            }
        }
        Handle.Queue.CustomerList.RemoveAt(0);
        Handle.Queue.UpdateLine();
        WaitBeforeNextCustomer = FixedWaitingTime;
    }

    private void PlayerSellItem()
    {
        CanvasObject.SetActive(true);
        InstantiatedObject = Instantiate(ShopObject, CanvasObject.transform, false);
        InstantiatedObject.GetComponentInChildren<ActualShopUI>().enabled = true;
        InstantiatedObject.GetComponentInChildren<ActualShopUI>().ThisUI = this;
        InstantiatedObject.GetComponentInChildren<ActualShopUI>().ItemsToSell = SellingItems;        
        Movement.Instance.enabled = false;
    }

    public void BuyItem(Item SellingItem)
    {
        if(MoneyManager.Instance.CurrentMoney >= SellingItem.ItemOfChoice.Cost)
        {
            MoneyManager.Instance.MinusMoney(SellingItem.ItemOfChoice.Cost);
            Inventory.AddItem(SellingItem.ItemOfChoice);
        }
    }
}
