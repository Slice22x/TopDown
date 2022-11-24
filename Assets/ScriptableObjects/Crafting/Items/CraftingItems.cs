using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CraftingItem", menuName = "Create CraftingItem")]
public class CraftingItems : ScriptableObject
{
    public string Name;
    public string Description;
    public int ID;
    public Sprite ItemIcon;
}
