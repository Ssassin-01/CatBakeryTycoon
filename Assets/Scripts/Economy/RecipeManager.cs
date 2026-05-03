using System;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private EconomyManager economyManager;

    private void Awake()
    {
        ResolveReferences();
    }

    public int GetRecipeLevel()
    {
        GameSaveData data = GetSaveData();
        return data != null ? data.recipeLevel : 1;
    }

    public double GetCurrentTapIncome()
    {
        if (economyManager == null)
        {
            ResolveReferences();
        }

        return economyManager != null ? economyManager.GetTapIncome() : 0d;
    }

    public double GetNextUpgradeCost()
    {
        return GetUpgradeCost(GetRecipeLevel());
    }

    public double GetUpgradeCost(int recipeLevel)
    {
        int safeLevel = Mathf.Max(1, recipeLevel);
        return Math.Floor(10d * Math.Pow(1.35d, safeLevel - 1));
    }

    public bool TryUpgradeRecipe()
    {
        ResolveReferences();

        GameSaveData data = GetSaveData();
        if (data == null)
        {
            Debug.LogWarning("[RecipeManager] Save data is not ready.");
            return false;
        }

        if (economyManager == null)
        {
            Debug.LogWarning("[RecipeManager] EconomyManager reference is missing.");
            return false;
        }

        double upgradeCost = GetUpgradeCost(data.recipeLevel);
        if (!economyManager.TrySpendMoney(upgradeCost))
        {
            Debug.Log("Not enough money");
            return false;
        }

        data.recipeLevel += 1;
        data.tapPower += 1d;

        economyManager.RecalculateIncome();

        if (gameManager != null)
        {
            gameManager.SaveNow();
        }

        return true;
    }

    private GameSaveData GetSaveData()
    {
        if (gameManager == null)
        {
            ResolveReferences();
        }

        return gameManager != null ? gameManager.SaveData : null;
    }

    private void ResolveReferences()
    {
        if (gameManager == null)
        {
            gameManager = GameManager.Instance != null
                ? GameManager.Instance
                : FindFirstObjectByType<GameManager>();
        }

        if (economyManager == null)
        {
            economyManager = gameManager != null && gameManager.Economy != null
                ? gameManager.Economy
                : FindFirstObjectByType<EconomyManager>();
        }
    }
}
