using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTrans : MonoBehaviour
{
    public Animator Anim;
    public static ScreenTrans Instance;

    public System.Action Act;

    public PlayerInfo Info;

    public delegate void HasFinished();
    public static event HasFinished TransDone;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void CallLevel(string SceneName)
    {
        Instance.StartCoroutine(Instance.LoadLevel(SceneName));
    }

    public static void CallLevel(int SceneIndex)
    {
        Instance.StartCoroutine(Instance.LoadLevel(SceneIndex));
    }

    public static void CallLevelTS()
    {
        if (!Instance.Info.HasSeenStory)
        {
            Instance.StartCoroutine(Instance.LoadLevel("Starting_House"));
        }
        if (Instance.Info.HasSeenStory)
        {
            Instance.StartCoroutine(Instance.LoadLevel("Village"));
        }
    }

    public void CallActionOnFinish()
    {
        if(SaveSystem.LoadData() != null)
        {
            Info.HasSeenStory = SaveSystem.LoadData().SeenStory;
            Info.Level = SaveSystem.LoadData().Level;
            Info.Money = SaveSystem.LoadData().Money;
            if(SaveSystem.LoadData().PlayerInv.Count > 0)
            {
                List<ItemScriptableObject> TempInv = new List<ItemScriptableObject>();
                foreach (string item in SaveSystem.LoadData().PlayerInv)
                {
                    TempInv.Add(ItemFromID.GetItemFromID(item).ItemOfChoice);
                }
                Info.Inv = TempInv;
            }

        }
        if (TransDone != null)
            TransDone();
    }

    public static IEnumerator MoveToSceneEnter(HouseInfo HInfo)
    {
        Instance.Anim.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SaveSystem.SaveData(Instance.Info);

        SceneManager.MoveGameObjectToScene(Movement.Instance.gameObject, SceneManager.GetSceneByName(HInfo.SceneName));

        Movement.Instance.transform.position = HInfo.EntrancePosition;

        SceneType.Instance.ThisScene = SceneType.TypeOfScene.House;

        DontDestroyOnLoad(Movement.Instance.gameObject);

        yield return new WaitForSeconds(0.4f);

        Instance.Anim.SetTrigger("End");
    }

    public static IEnumerator MoveToSceneExit(HouseInfo HInfo)
    {
        Instance.Anim.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SaveSystem.SaveData(Instance.Info);

        SceneManager.MoveGameObjectToScene(Movement.Instance.gameObject, SceneManager.GetSceneByName("Village"));

        Movement.Instance.transform.position = HInfo.ExitPosition;

        SceneType.Instance.ThisScene = SceneType.TypeOfScene.Village;

        DontDestroyOnLoad(Movement.Instance.gameObject);

        yield return new WaitForSeconds(0.4f);

        Instance.Anim.SetTrigger("End");
    }

    public static void MoveToSceneEnter(HouseInfo HInfo,GameObject ToMove)
    {
        SceneManager.MoveGameObjectToScene(ToMove, SceneManager.GetSceneByName(HInfo.SceneName));

        ToMove.transform.position = HInfo.EntrancePosition;
    }

    public static void MoveToSceneExit(HouseInfo HInfo,GameObject ToMove)
    {
        SceneManager.MoveGameObjectToScene(ToMove, SceneManager.GetSceneByName("Village"));

        ToMove.transform.position = HInfo.ExitPosition;
    }

    public IEnumerator LoadLevel(string SceneName)
    {
        SoundManager.StopAll();
        Anim.SetTrigger("Start");

        if(SceneType.Instance != null)
        {
            if (SceneType.Instance.ThisScene == SceneType.TypeOfScene.Village)
                FindObjectOfType<LoadHouses>().UnloadHouses();
        }


        yield return new WaitForSeconds(1);

        SaveSystem.SaveData(Info);

        SceneManager.LoadScene(SceneName);
    }
    public IEnumerator LoadLevel(int SceneIndex)
    {
        SoundManager.StopAll();
        Anim.SetTrigger("Start");

        if (SceneType.Instance != null)
        {
            if (SceneType.Instance.ThisScene == SceneType.TypeOfScene.Village)
                FindObjectOfType<LoadHouses>().UnloadHouses();
        }


        yield return new WaitForSeconds(1);

        SaveSystem.SaveData(Info);

        SceneManager.LoadScene(SceneIndex);
    }
}
