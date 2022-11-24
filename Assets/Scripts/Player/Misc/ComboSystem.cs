using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ComboSystem : MonoBehaviour
{

    public int Combo;
    public float ComboTimer;
    private const float FixedComboTimer = 5f;
    public static ComboSystem Instance { get; private set; }
    public HealthBar ComboTimeBar;
    void Start()
    {
        Instance = this;
        ComboTimeBar = GetComponent<HealthBar>();
        ComboTimeBar.SetMaxValue(FixedComboTimer);
    }



    // Update is called once per frame
    void Update()
    {
        if(Combo > 0)
        {
            ComboTimer -= Time.smoothDeltaTime;
            ComboTimeBar.SetValue(ComboTimer);
            if(ComboTimer <= 0)
            {
                Combo = 0;
            }
        }


        if (GameObject.Find("ComboText") != null) GameObject.Find("ComboText").GetComponent<TMP_Text>().text = "Combo: " +  Combo.ToString();
    }

    public static void AddToCombo(int ComboAdder)
    {
        if(Instance.ComboTimer <= 0 || Instance.ComboTimer > 0)
        {
            if (ComboAdder > 0)
            {
                Instance.Combo += ComboAdder;
                Instance.ComboTimer = FixedComboTimer;
            }
        }
    }

    public static void AddToCombo()
    {
        if (Instance.ComboTimer <= 0 || Instance.ComboTimer > 0)
        {
            Instance.Combo++;
            Instance.ComboTimer = FixedComboTimer;
        }
    }
}
