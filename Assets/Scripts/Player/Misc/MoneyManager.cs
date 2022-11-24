using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int CurrentMoney;
    public int MoneyConstant;
    public static MoneyManager Instance { get; private set; }
    void Start()
    {
        Instance = this;
        CurrentMoney = PlayerPrefs.GetInt("CurrentMoney");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetMoneyConstant()
    {
        MoneyConstant = 0;
    }

    public void SetMoneyConstant(int Money)
    {
        MoneyConstant = Money;
    }

    public void AddMoney(int Money)
    {
        CurrentMoney += Money;
    }

    public void MinusMoney(int Money)
    {
        CurrentMoney -= Money;
    }
}
