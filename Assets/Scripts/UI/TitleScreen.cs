using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public LeanTweenType Ease;
    void Start()
    {
        RotateRight();
        SoundManager.Play("Night_Theme");
    }

    public void RotateLeft()
    {
        LeanTween.rotateZ(gameObject, -10f, 1f).setOnComplete(RotateRight).setEase(Ease);
    }
    
    public void RotateRight()
    {
        LeanTween.rotateZ(gameObject, 10f, 1f).setOnComplete(RotateLeft).setEase(Ease);
    }
}
