using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUpgradePanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RecipeManager recipeManager;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI recipeLevelText;
    [SerializeField] private TextMeshProUGUI tapIncomeText;
    [SerializeField] private TextMeshProUGUI upgradeCostText;

    [Header("Buttons")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        ResolveReferences();

        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Close);
        }
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Open()
    {
        gameObject.SetActive(true);
        Refresh();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Refresh()
    {
        ResolveReferences();

        if (recipeManager == null)
        {
            SetText(recipeLevelText, "레시피 Lv. -");
            SetText(tapIncomeText, "현재 터치 수익: +0원");
            SetText(upgradeCostText, "업그레이드 비용: -");
            return;
        }

        SetText(recipeLevelText, $"레시피 Lv. {recipeManager.GetRecipeLevel()}");
        SetText(tapIncomeText, $"현재 터치 수익: +{FormatMoney(recipeManager.GetCurrentTapIncome())}원");
        SetText(upgradeCostText, $"업그레이드 비용: {FormatMoney(recipeManager.GetNextUpgradeCost())}원");
    }

    private void OnUpgradeButtonClicked()
    {
        if (recipeManager == null)
        {
            ResolveReferences();
        }

        if (recipeManager == null)
        {
            Debug.LogWarning("[RecipeUpgradePanel] RecipeManager reference is missing.");
            return;
        }

        recipeManager.TryUpgradeRecipe();
        Refresh();
    }

    private void ResolveReferences()
    {
        if (recipeManager == null)
        {
            recipeManager = FindFirstObjectByType<RecipeManager>();
        }
    }

    private static void SetText(TextMeshProUGUI text, string value)
    {
        if (text != null)
        {
            text.text = value;
        }
    }

    private static string FormatMoney(double value)
    {
        if (value < 1000d)
        {
            return value.ToString("0");
        }

        return value.ToString("N0");
    }
}
