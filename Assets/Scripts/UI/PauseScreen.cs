using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public GameObject PauseObject;

    public TMP_Text FalseText;
    public TMP_Text Return;

    public delegate void HasCalled();
    public static event HasCalled Called;

    public static PauseScreen Instance;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.isPressed)
        {
            if (Dialogue.Instance.IsTalking)
                return;

            PauseObject.SetActive(true);
            if (Movement.Instance != null)
            {
                Movement.Instance.InAction = true;
            }
            if (StoryMovement.Instance != null)
            {
                StoryMovement.Instance.InAction = true;
            }
            Time.timeScale = 0f;
            switch (SceneType.Instance.ThisScene)
            {
                case SceneType.TypeOfScene.Boss:
                    FalseText.text = "Resume";
                    Return.text = "Retrun";
                    break;
                case SceneType.TypeOfScene.Level:
                    FalseText.text = "Resume";
                    Return.text = "Retrun";
                    break;
                case SceneType.TypeOfScene.House:
                    FalseText.text = "Resume";
                    Return.text = "Title";
                    break;
                case SceneType.TypeOfScene.Village:
                    FalseText.text = "Resume";
                    Return.text = "Title";
                    break;
            }
        }
        if(PauseObject.activeSelf == true)
        {
            if(Movement.Instance != null)
            {
                Movement.Instance.InAction = true;
            }
            if (StoryMovement.Instance != null)
            {
                StoryMovement.Instance.InAction = true;
            }
            Time.timeScale = 0f;
            Cursor.visible = true;
        }
    }

    public void SetFalse()
    {
        PauseObject.SetActive(false);
        if (Movement.Instance != null)
        {
            Movement.Instance.InAction = false;
        }
        if (StoryMovement.Instance != null)
        {
            StoryMovement.Instance.InAction = false;
        }
        Time.timeScale = 1f;
    } 

    public void Leave()
    {
        switch (SceneType.Instance.ThisScene)
        {
            case SceneType.TypeOfScene.Boss:
                if (WeaponManager.Instance.CurrentWeapon != null) WeaponManager.Instance.DropWeapon();
                ScreenTrans.CallLevel("Village");
                if (Called != null)
                {
                    Called();
                }
                Level.ResetLevelInfo(PortalInfo.Instance.ThisLevel, true);
                break;
            case SceneType.TypeOfScene.Level:
                if (WeaponManager.Instance.CurrentWeapon != null) WeaponManager.Instance.DropWeapon();
                ScreenTrans.CallLevel("Village");
                if (Called != null)
                {
                    Called();
                }
                Level.ResetLevelInfo(PortalInfo.Instance.ThisLevel, true);
                break;
            case SceneType.TypeOfScene.House:
                ScreenTrans.CallLevel("TitleScreen");
                break;
            case SceneType.TypeOfScene.Village:
                ScreenTrans.CallLevel("TitleScreen");
                break;
        }
        SetFalse();
    }
}
