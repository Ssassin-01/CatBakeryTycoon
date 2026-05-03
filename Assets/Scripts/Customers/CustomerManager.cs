using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [Header("Satisfaction")]
    [SerializeField] private float waitingWarningThreshold = 10f;
    [SerializeField] private float satisfactionLossPerSecond = 1.5f;
    [SerializeField] private float satisfactionRecoveryPerSecond = 0.5f;

    [Header("Regular Customers")]
    [SerializeField] private float baseRegularCustomerChance = 0.01f;

    private GameSaveData saveData;
    private float regularCustomerProgress;

    public float CurrentServedCustomersPerSecond { get; private set; }
    public int DisplayWaitingCustomerCount => saveData == null
        ? 0
        : Mathf.FloorToInt(saveData.waitingCustomerCount);

    public void Initialize(GameSaveData data)
    {
        saveData = data;
        CurrentServedCustomersPerSecond = 0f;
        regularCustomerProgress = 0f;
    }

    public void Tick(float deltaTime)
    {
        if (saveData == null || deltaTime <= 0f)
        {
            CurrentServedCustomersPerSecond = 0f;
            return;
        }

        float arrivals = Mathf.Max(0f, saveData.customerVisitRate) * deltaTime;
        saveData.waitingCustomerCount += arrivals;

        float serviceCapacity = Mathf.Max(0f, saveData.customerServeRate) * deltaTime;
        float servedCustomers = Mathf.Min(saveData.waitingCustomerCount, serviceCapacity);
        saveData.waitingCustomerCount = Mathf.Max(0f, saveData.waitingCustomerCount - servedCustomers);

        CurrentServedCustomersPerSecond = servedCustomers / deltaTime;

        UpdateRegularCustomers(servedCustomers);
        UpdateSatisfaction(deltaTime);
    }

    private void UpdateRegularCustomers(float servedCustomers)
    {
        if (servedCustomers <= 0f)
        {
            return;
        }

        float satisfactionBonus = saveData.customerSatisfaction >= 90f ? 1.5f : 1f;
        regularCustomerProgress += servedCustomers * baseRegularCustomerChance * satisfactionBonus;

        if (regularCustomerProgress < 1f)
        {
            return;
        }

        int gainedRegulars = Mathf.FloorToInt(regularCustomerProgress);
        saveData.regularCustomers += gainedRegulars;
        regularCustomerProgress -= gainedRegulars;
    }

    private void UpdateSatisfaction(float deltaTime)
    {
        if (saveData.waitingCustomerCount > waitingWarningThreshold)
        {
            saveData.customerSatisfaction -= satisfactionLossPerSecond * deltaTime;
        }
        else
        {
            saveData.customerSatisfaction += satisfactionRecoveryPerSecond * deltaTime;
        }

        saveData.customerSatisfaction = Mathf.Clamp(saveData.customerSatisfaction, 0f, 100f);
    }
}
