using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public GameObject Player;
    public bool GameEndend;
    public bool CancelTut;

    void Start()
    {

    }

    public void LoadScene(string Name)
    {
        ScreenTrans.CallLevel(Name);
        if(SceneType.Instance.ThisScene == SceneType.TypeOfScene.Level)
        {
            if(Name == "WinScreen")
            {
                Movement.Instance.Info.Level = SceneType.Instance.NextLevelNumber;
                Level.AddToTotal(PortalInfo.Instance.ThisLevel);
                PortalInfo.Instance.ThisLevel.LevelIndex = SceneManager.GetActiveScene().buildIndex;
                Movement.Instance.LevelBefore = PortalInfo.Instance.ThisLevel;
            }
            if (WeaponManager.Instance.CurrentWeapon != null)
            {
                WeaponManager.Instance.DropWeapon();
            }
        }
    }

    public void LoadScene(int Index)
    {
        ScreenTrans.CallLevel(Index);
        if (SceneType.Instance.ThisScene == SceneType.TypeOfScene.Level)
        {
            if (Index == SceneManager.GetSceneByName("WinScreen").buildIndex)
            {
                Movement.Instance.Info.Level = SceneType.Instance.NextLevelNumber;
                Level.AddToTotal(PortalInfo.Instance.ThisLevel);
                PortalInfo.Instance.ThisLevel.LevelIndex = SceneManager.GetActiveScene().buildIndex;
                Movement.Instance.LevelBefore = PortalInfo.Instance.ThisLevel;
            }
            if (WeaponManager.Instance.CurrentWeapon != null)
            {
                WeaponManager.Instance.DropWeapon();
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Level.ResetLevelInfo(PortalInfo.Instance.ThisLevel, false);
    }

    public void RunScene(string Name)
    {
        SceneManager.LoadScene(Name);
    }

    public void ExitScene()
    {
        Application.Quit();
    }

    void Update()
    {
        if(ScreenTrans.Instance != null)
        {
            if (GetComponent<SceneType>().ThisScene == SceneType.TypeOfScene.Village)
            {
                if (Keyboard.current.escapeKey.isPressed)
                {
                    Application.Quit();
                }
            }
        }
    }
}
