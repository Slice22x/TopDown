using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

public class Item : MonoBehaviour
{

#if UNITY_EDITOR
    [MenuItem("GameObject/2D Object/NewInGameItem")]
    public static void AddNewGameItem()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("NewItem/NewInGameItem"));
    }
#endif

    public ItemScriptableObject ItemOfChoice;
    public CraftingItems Items;
    public static int IsBought;
    public int IsBoughtVar;

    private void Update()
    {
        IsBought = IsBoughtVar;
    }

    public void SetTrue()
    {
        IsBought = 1;
    }
}
