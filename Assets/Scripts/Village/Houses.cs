using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Houses : MonoBehaviour
{
    public HouseInfo Info;
    Detection Detect;

    private void Awake()
    {
        gameObject.name = Info.HouseName;
        //SceneManager.LoadSceneAsync(Info.name);
        Detect = GetComponent<Detection>();
    }

    private void Update()
    {
        if (Detect.Detected)
        {
            if(Detect.ObjectsInArea != null && Detect.ObjectsInArea.CompareTag("Villager"))
            {
                NPCAI VillagerToMove = Detect.ObjectsInArea.GetComponent<NPCAI>();
                if (VillagerToMove.NPC.House != Info)
                    return;
                VillagerToMove.SwitchState(NPCAI.State.Waiting);
                if (SceneManager.GetSceneByName(Info.HouseName).IsValid())
                {
                    ScreenTrans.MoveToSceneEnter(Info, VillagerToMove.gameObject);
                }
            }
            if (Detect.ObjectsInArea.CompareTag("Player"))
            {
                Movement.Instance.ActionPrompt = true;
                Movement.Instance.InShopArea = true;
                if (Movement.Instance.Interact.triggered)
                {
                    if (SceneManager.GetSceneByName(Info.HouseName).IsValid())
                    {
                        StartCoroutine(ScreenTrans.MoveToSceneEnter(Info));
                    }
                }
            }

        }

    }
}
