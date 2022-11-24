using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int Money;
    //Levels
    public int Level;
    public bool SeenStory;
    public List<string> PlayerInv = new List<string>();
    public int Rep;

    public PlayerData(PlayerInfo Info)
    {
        if(Info != null)
        {
            Level = Info.Level;
            SeenStory = Info.HasSeenStory;
            Money = Info.Money;
            Rep = Info.Reputation;
            if(Inventory.CurrentInv().Count > 0)
            {
                Debug.Log("Checked");
                PlayerInv = Inventory.ConvertToStrArray();
                if (Info.Inv.Count > 0)
                {
                    foreach (ItemScriptableObject item in Info.Inv)
                    {
                        PlayerInv.Add(item.ID);
                    }
                }
            }
            else
            {
                if (Info.Inv.Count > 0)
                {
                    foreach (ItemScriptableObject item in Info.Inv)
                    {
                        PlayerInv.Add(item.ID);
                    }
                }
            }

            Inventory.LoadNewInventory(new List<ItemScriptableObject>());
        }
    }
}
