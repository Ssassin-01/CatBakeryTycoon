using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EconomyManager economyManager;

    [Header("Top HUD Text")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text tapIncomeText;
    [SerializeField] private TMP_Text autoIncomeText;
    [SerializeField] private TMP_Text waitingCustomerText;
    [SerializeField] private TMP_Text regularCustomerText;
    [SerializeField] private TMP_Text ratingText;

    [Header("Buttons")]
    [SerializeField] private Button settingsButton;

    private void Awake()
    {
        if (economyManager == null)
        {
            economyManager = FindFirstObjectByType<EconomyManager>();
        }

        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        }
    }

    public void Refresh(GameSaveData data)
    {
        if (data == null)
        {
            SetText(moneyText, "매출 0원");
            SetText(tapIncomeText, "터치 +0원");
            SetText(autoIncomeText, "초당 0원/s");
            SetText(waitingCustomerText, "대기 0명");
            SetText(regularCustomerText, "단골 0명");
            SetText(ratingText, "별점 1.0");
            return;
        }

        double tapIncome = economyManager != null
            ? economyManager.GetTapIncome()
            : data.tapPower * data.globalMultiplier;

        SetText(moneyText, $"매출 {FormatMoney(data.money)}원");
        SetText(tapIncomeText, $"터치 +{FormatMoney(tapIncome)}원");
        SetText(autoIncomeText, $"초당 {FormatMoney(data.autoIncomePerSecond)}원/s");
        SetText(waitingCustomerText, $"대기 {Mathf.FloorToInt(data.waitingCustomerCount)}명");
        SetText(regularCustomerText, $"단골 {data.regularCustomers}명");
        SetText(ratingText, $"별점 {data.rating:0.0}");
    }

    private static void SetText(TMP_Text text, string value)
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

    private void OnSettingsButtonClicked()
    {
        Debug.Log("[HUDController] Settings button clicked. Settings popup will be implemented later.");
    }
}
