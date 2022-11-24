using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public HouseInfo ThisHouse;

    public Detection Detect;

    private void Update()
    {
        if (Detect.Detected)
        {
            if(Detect.ObjectsInArea != null && Detect.ObjectsInArea.CompareTag("Villager"))
            {
                Detect.ObjectsInArea.gameObject.GetComponent<NPCAI>().SwitchState(NPCAI.State.Shopping);
                ScreenTrans.MoveToSceneExit(ThisHouse, Detect.ObjectsInArea.gameObject);
            }
            if (Detect.ObjectsInArea.CompareTag("Player"))
            {
                Movement.Instance.ActionPrompt = true;
                if (Movement.Instance.Interact.triggered)
                    StartCoroutine(ScreenTrans.MoveToSceneExit(ThisHouse));
            }
        }
        if (!Detect.Detected)
        {
            Movement.Instance.ActionPrompt = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
