using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider Slide;
    public Text HealthText;

    public bool Player;

    private void Awake()
    {
        ScreenTrans.TransDone += ScreenTrans_TransDone;
        PauseScreen.Called += PauseScreen_Called;
    }

    private void PauseScreen_Called()
    {
        ScreenTrans.TransDone -= ScreenTrans_TransDone;
        PauseScreen.Called -= PauseScreen_Called;
    }

    private void ScreenTrans_TransDone()
    {
        if (Player && SceneType.Instance.ThisScene == SceneType.TypeOfScene.Level)
        {
            SetValue(Health.Instance.HP);
            SetMaxValue(Health.Instance.MaxHP);
            HealthText.text = Health.Instance.HP.ToString() + "/" + Health.Instance.MaxHP.ToString();
        }
    }

    public void SetMaxValue(int MaxHealth)
    {
        Slide.maxValue = MaxHealth;
    }

    public void SetValue(int Heal)
    {
        Slide.value = Heal;
    }

    public void SetMaxValue(float MaxValue)
    {
        Slide.maxValue = MaxValue;
        Slide.value = MaxValue;
    }

    public void SetValue(float Value)
    {
        Slide.value = Value;
    }

    public void SetText(string Values)
    {
        HealthText.text = Values;
    }

    public void Update()
    {
        if (Player)
        {
            SetValue(Health.Instance.HP);
            HealthText.text = Health.Instance.HP.ToString() + "/" + Health.Instance.MaxHP.ToString();
        }
    }
}
