using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneType : MonoBehaviour
{
    public enum TypeOfScene
    {
        Boss,
        Level,
        Village,
        House,
        Misc
    }

    public TypeOfScene ThisScene;

    public static SceneType Instance { get; private set; }

    public int NextLevelNumber;

    public Transform SpawnPos;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(ThisScene == TypeOfScene.Village)
        {
            Movement.Instance.transform.position = SpawnPos.position;
        }
        if (ThisScene == TypeOfScene.Level || ThisScene == TypeOfScene.Boss)
        {
            Movement.Instance.transform.position = SpawnPos.position;
        }
    }
}
