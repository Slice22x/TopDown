using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    public LeanTweenType Ease;
    void Start()
    {
        RotateRight();
    }


    public void RotateLeft()
    {
        LeanTween.rotateZ(gameObject, -5f, 0.5f).setOnComplete(RotateRight).setEase(Ease);
    }

    public void RotateRight()
    {
        LeanTween.rotateZ(gameObject, 5f, 0.5f).setOnComplete(RotateLeft).setEase(Ease);
    }

    public void Play()
    {
        GameObject.Find("MainScreen").GetComponent<Animator>().Play("MScreenExit");
        GetComponent<Animator>().Play("Falling_Exit");
    }

    public void Fall()
    {
        ScreenTrans.CallLevelTS();
    }

    public void CallMScreenEnter()
    {
        GameObject.Find("MainScreen").GetComponent<Animator>().Play("MainScreen");
    }

    public void CallMScreenExitTS()
    {
        
    }

    public void CallMScreenExit()
    {
        GameObject.Find("MainScreen").GetComponent<Animator>().Play("MScreenExit");
        GameObject.Find("OptionsScreen").GetComponent<Animator>().Play("MainScreen");
    }

    public void CallOScreenExit()
    {
        GameObject.Find("OptionsScreen").GetComponent<Animator>().Play("MScreenExit");
        GameObject.Find("MainScreen").GetComponent<Animator>().Play("MainScreen");
    }
}
