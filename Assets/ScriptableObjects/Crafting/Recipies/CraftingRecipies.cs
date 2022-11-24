using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipie", menuName = "Create Recipie")]
public class CraftingRecipies : ScriptableObject
{
    public ItemScriptableObject Output;

    [Header("Matrix")]
    [Space]
    public CraftingItems Item00;
    public CraftingItems Item01;
    public CraftingItems Item02;
    [Space]
    public CraftingItems Item10;
    public CraftingItems Item11;
    public CraftingItems Item12;
    [Space]
    public CraftingItems Item20;
    public CraftingItems Item21;
    public CraftingItems Item22;

    public CraftingItems GetItem(int x, int y)
    {
        if (x == 0 && y == 0) return Item00;
        if (x == 1 && y == 0) return Item10;
        if (x == 2 && y == 0) return Item20;

        if (x == 0 && y == 1) return Item01;
        if (x == 1 && y == 1) return Item11;
        if (x == 2 && y == 1) return Item21;

        if (x == 0 && y == 2) return Item02;
        if (x == 1 && y == 2) return Item12;
        if (x == 2 && y == 2) return Item22;

        return null;
    }
}
