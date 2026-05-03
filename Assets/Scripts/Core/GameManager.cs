using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Managers")]
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private EconomyManager economyManager;
    [SerializeField] private CustomerManager customerManager;
    [SerializeField] private HUDController hudController;

    [Header("Save")]
    [SerializeField] private float autoSaveIntervalSeconds = 10f;

    private GameSaveData saveData;
    private float autoSaveTimer;
    private bool initialized;

    public GameSaveData SaveData => saveData;
    public EconomyManager Economy => economyManager;
    public CustomerManager Customers => customerManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        ResolveReferences();
    }

    private void Start()
    {
        LoadAndInitialize();
    }

    private void Update()
    {
        if (!initialized)
        {
            return;
        }

        float deltaTime = Time.deltaTime;
        customerManager.Tick(deltaTime);
        economyManager.Tick(deltaTime);
        hudController.Refresh(saveData);

        autoSaveTimer += deltaTime;
        if (autoSaveTimer >= autoSaveIntervalSeconds)
        {
            SaveNow();
            autoSaveTimer = 0f;
        }
    }

    public void SaveNow()
    {
        if (saveManager == null || saveData == null)
        {
            return;
        }

        saveManager.Save(saveData);
    }

    private void LoadAndInitialize()
    {
        if (saveManager == null)
        {
            Debug.LogError("[GameManager] SaveManager reference is missing.");
            return;
        }

        saveData = saveManager.Load();
        saveData.EnsureValidValues();

        if (customerManager == null)
        {
            Debug.LogError("[GameManager] CustomerManager reference is missing.");
            return;
        }

        if (economyManager == null)
        {
            Debug.LogError("[GameManager] EconomyManager reference is missing.");
            return;
        }

        if (hudController == null)
        {
            Debug.LogError("[GameManager] HUDController reference is missing.");
            return;
        }

        customerManager.Initialize(saveData);
        economyManager.Initialize(saveData, customerManager);
        hudController.Refresh(saveData);

        autoSaveTimer = 0f;
        initialized = true;
    }

    private void ResolveReferences()
    {
        if (saveManager == null)
        {
            saveManager = FindFirstObjectByType<SaveManager>();
        }

        if (economyManager == null)
        {
            economyManager = FindFirstObjectByType<EconomyManager>();
        }

        if (customerManager == null)
        {
            customerManager = FindFirstObjectByType<CustomerManager>();
        }

        if (hudController == null)
        {
            hudController = FindFirstObjectByType<HUDController>();
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveNow();
        }
    }

    private void OnApplicationQuit()
    {
        SaveNow();
    }
}
