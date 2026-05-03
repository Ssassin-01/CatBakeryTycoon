using System;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [SerializeField] private CustomerManager customerManager;

    private GameSaveData saveData;

    public double CurrentTapIncome { get; private set; }
    public double CurrentAutoIncomePerSecond { get; private set; }

    public void Initialize(GameSaveData data, CustomerManager customers)
    {
        saveData = data;

        if (customers != null)
        {
            customerManager = customers;
        }

        RecalculateIncome();
    }

    public void Tick(float deltaTime)
    {
        if (saveData == null)
        {
            return;
        }

        RecalculateIncome();

        if (deltaTime <= 0f || CurrentAutoIncomePerSecond <= 0d)
        {
            return;
        }

        AddMoney(CurrentAutoIncomePerSecond * deltaTime);
    }

    public double AddTapIncome()
    {
        if (saveData == null)
        {
            Debug.LogWarning("[EconomyManager] Save data is not initialized yet.");
            return 0d;
        }

        RecalculateIncome();
        AddMoney(CurrentTapIncome);
        return CurrentTapIncome;
    }

    public void AddMoney(double amount)
    {
        if (saveData == null || amount <= 0d || double.IsNaN(amount) || double.IsInfinity(amount))
        {
            return;
        }

        saveData.money += amount;
        saveData.totalEarnedMoney += amount;
    }

    public void RecalculateIncome()
    {
        if (saveData == null)
        {
            CurrentTapIncome = 0d;
            CurrentAutoIncomePerSecond = 0d;
            return;
        }

        double multiplier = Math.Max(0d, saveData.globalMultiplier);
        CurrentTapIncome = Math.Max(0d, saveData.tapPower) * multiplier;

        float servedCustomersPerSecond = customerManager != null
            ? customerManager.CurrentServedCustomersPerSecond
            : 0f;

        CurrentAutoIncomePerSecond = Math.Max(0f, servedCustomersPerSecond)
            * Math.Max(0d, saveData.averageSpendPerCustomer)
            * multiplier;

        saveData.autoIncomePerSecond = CurrentAutoIncomePerSecond;
    }
}
