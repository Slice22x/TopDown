using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinCanvas : MonoBehaviour
{
    public Level LevelBefore;

    public GameObject Deaths, Cured, Dead, Shots, Damage;
    public TMP_Text Title;

    private void Awake()
    {
        LevelBefore = Movement.Instance.LevelBefore;
        foreach (MonoBehaviour item in FindObjectsOfType<MonoBehaviour>())
        {
            if (item.gameObject.scene.buildIndex == -1)
                if(item.GetComponent<SoundManager>() == null)
                    Destroy(item.gameObject);
        }
    }

    void Start()
    {
        Destroy(Movement.Instance.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Title.text = LevelBefore.LevelID + " Cleared";
        Deaths.GetComponentInChildren<TMP_Text>().text = LevelBefore.Deaths.ToString();
        Cured.GetComponentInChildren<TMP_Text>().text = LevelBefore.EnemiesCured.ToString();
        Dead.GetComponentInChildren<TMP_Text>().text = LevelBefore.EnemiesDead.ToString();
        Shots.GetComponentInChildren<TMP_Text>().text = LevelBefore.BulletsShot.ToString();
        Damage.GetComponentInChildren<TMP_Text>().text = LevelBefore.DamageTaken.ToString();
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(LevelBefore.LevelIndex + 1));
        Level.ResetLevelInfo(LevelBefore, true);
    }
    public void LoadVillage()
    {
        FindObjectOfType<Exit>().LoadScene("Village");
        Level.ResetLevelInfo(LevelBefore, true);
    }
    public void Retry()
    {
        StartCoroutine(LoadLevel(LevelBefore.LevelIndex));
        Level.ResetLevelInfo(LevelBefore, true);
    }

    public IEnumerator LoadLevel(int SceneIndex)
    {
        ScreenTrans.Instance.Anim.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SaveSystem.SaveData(ScreenTrans.Instance.Info);

        SceneManager.LoadScene(SceneIndex);
    }
}
