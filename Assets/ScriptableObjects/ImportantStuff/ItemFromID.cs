using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFromID : MonoBehaviour
{
    [SerializeField] private List<ItemScriptableObject> EveryItem = new List<ItemScriptableObject>();
    public static ItemFromID Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {

    }

    [SerializeField]
    public static Item GetItemFromID(string ID)
    {
        Item NewItem = Instance.EveryItem.Find(item => item.ID == ID).ItemGameObject.GetComponent<Item>();
        return NewItem;
    }

    public static Item GetItemFromName(string Name)
    {
        Item NewItem = Instance.EveryItem.Find(item => item.Name == Name).ItemGameObject.GetComponent<Item>();
        return NewItem;
    }
}
