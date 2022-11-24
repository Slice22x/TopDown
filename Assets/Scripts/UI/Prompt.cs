using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TypeOfPrompt
{
    Item,
    Action
}

public class Prompt : MonoBehaviour
{
    public Image ItemImage;
    public TMP_Text HeaderText, YesText, NoText;
    public TypeOfPrompt TypePrompt;
    public NPCAI DirectedTo;
    public static Prompt Instance;
    public bool NoCarryOn;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        switch (TypePrompt)
        {
            case TypeOfPrompt.Item:
                ItemImage.gameObject.SetActive(true);
                break;
            case TypeOfPrompt.Action:
                ItemImage.gameObject.SetActive(false);
                break;
        }
        LeanTween.scale(gameObject, Vector3.one, 1f);
    }

    public void GiveToNPC()
    {
        DirectedTo.NPC.LoveForPlayer += int.Parse(YesText.text);
        LeanTween.scale(gameObject, Vector3.zero, 1f).setOnComplete(() => 
        {
            Destroy(gameObject);
        });
        NoCarryOn = false;
    }

    public void NoToNPC()
    {
        DirectedTo.NPC.LoveForPlayer += int.Parse(NoText.text);
        LeanTween.scale(gameObject, Vector3.zero, 1f).setOnComplete(() =>
        {
            Destroy(gameObject);
        });
        NoCarryOn = false;
    }

}
