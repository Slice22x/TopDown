using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyFinder : MonoBehaviour
{
    public List<TypeOfEnemy> EveryEnemy = new List<TypeOfEnemy>();
    public static EnemyFinder Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public TypeOfEnemy GetEnemyfromName(string ID)
    {
        return EveryEnemy.Find(item => item.ID == ID);
    }

    private void Update()
    {
        if (Keyboard.current.leftAltKey.wasPressedThisFrame)
        {
            if(SceneType.Instance.ThisScene == SceneType.TypeOfScene.Level)
            {
                PauseScreen.Instance.gameObject.SetActive(!PauseScreen.Instance.gameObject.activeSelf);
            }

        }
    }
}
