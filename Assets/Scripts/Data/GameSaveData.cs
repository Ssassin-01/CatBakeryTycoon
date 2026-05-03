using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSaveData
{
    public double money;
    public double totalEarnedMoney;
    public double tapPower;
    public double autoIncomePerSecond;
    public double globalMultiplier;

    public int regularCustomers;
    public float rating;
    public int recipeLevel;
    public int shopLevel;
    public int catSlotCount;

    public float waitingCustomerCount;
    public float customerVisitRate;
    public float customerServeRate;
    public double averageSpendPerCustomer;
    public float customerSatisfaction;

    public List<int> unlockedMenuIds = new List<int>();
    public List<int> hiredCatIds = new List<int>();
    public List<int> equippedCatIds = new List<int>();

    public string lastSaveTime;

    public static GameSaveData CreateDefault()
    {
        GameSaveData data = new GameSaveData
        {
            money = 0d,
            totalEarnedMoney = 0d,
            tapPower = 1d,
            autoIncomePerSecond = 0d,
            globalMultiplier = 1d,
            regularCustomers = 0,
            rating = 1f,
            recipeLevel = 1,
            shopLevel = 1,
            catSlotCount = 2,
            waitingCustomerCount = 0f,
            customerVisitRate = 0.2f,
            customerServeRate = 0.2f,
            averageSpendPerCustomer = 10d,
            customerSatisfaction = 100f,
            unlockedMenuIds = new List<int> { 1 },
            hiredCatIds = new List<int>(),
            equippedCatIds = new List<int>(),
            lastSaveTime = DateTime.UtcNow.ToString("O")
        };

        return data;
    }

    public void EnsureValidValues()
    {
        if (tapPower <= 0d)
        {
            tapPower = 1d;
        }

        if (globalMultiplier <= 0d)
        {
            globalMultiplier = 1d;
        }

        if (recipeLevel <= 0)
        {
            recipeLevel = 1;
        }

        if (shopLevel <= 0)
        {
            shopLevel = 1;
        }

        if (catSlotCount <= 0)
        {
            catSlotCount = 2;
        }

        customerVisitRate = Mathf.Max(0f, customerVisitRate);
        customerServeRate = Mathf.Max(0f, customerServeRate);
        waitingCustomerCount = Mathf.Max(0f, waitingCustomerCount);
        customerSatisfaction = Mathf.Clamp(customerSatisfaction, 0f, 100f);
        rating = Mathf.Clamp(rating <= 0f ? 1f : rating, 1f, 5f);

        if (averageSpendPerCustomer <= 0d)
        {
            averageSpendPerCustomer = 10d;
        }

        if (unlockedMenuIds == null)
        {
            unlockedMenuIds = new List<int>();
        }

        if (unlockedMenuIds.Count == 0)
        {
            unlockedMenuIds.Add(1);
        }

        if (hiredCatIds == null)
        {
            hiredCatIds = new List<int>();
        }

        if (equippedCatIds == null)
        {
            equippedCatIds = new List<int>();
        }

        if (string.IsNullOrWhiteSpace(lastSaveTime))
        {
            lastSaveTime = DateTime.UtcNow.ToString("O");
        }
    }
}
