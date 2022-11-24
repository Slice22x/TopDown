using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurInteractions : MonoBehaviour
{
   [SerializeField] private TypeOfDialogue InteractDialogue;
    public bool Interacted;

    private void Start()
    {
        LeanTween.color(gameObject, Color.yellow, 0.7f).setOnComplete(Return);
    }

    void Go()
    {
        if (!Interacted)
        {
            LeanTween.color(gameObject, Color.yellow, 0.7f).setOnComplete(Return);
        }
    }

    void Return()
    {
        LeanTween.color(gameObject, Color.white, 0.7f).setOnComplete(Go);
    }

    public void Interact()
    {
        Dialogue.MakeNewBox(InteractDialogue,gameObject);
        Interacted = true;
    }
}
