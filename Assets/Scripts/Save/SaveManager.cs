using System;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private string saveFileName = "cat_bakery_save.json";

    public string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

    public GameSaveData Load()
    {
        try
        {
            if (!File.Exists(SavePath))
            {
                return GameSaveData.CreateDefault();
            }

            string json = File.ReadAllText(SavePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return GameSaveData.CreateDefault();
            }

            GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);
            if (data == null)
            {
                return GameSaveData.CreateDefault();
            }

            data.EnsureValidValues();
            return data;
        }
        catch (Exception exception)
        {
            Debug.LogError($"[SaveManager] Failed to load save file. A default save will be used.\n{exception}");
            return GameSaveData.CreateDefault();
        }
    }

    public void Save(GameSaveData data)
    {
        if (data == null)
        {
            Debug.LogWarning("[SaveManager] Tried to save null data.");
            return;
        }

        try
        {
            data.EnsureValidValues();
            data.lastSaveTime = DateTime.UtcNow.ToString("O");

            string directory = Path.GetDirectoryName(SavePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
        }
        catch (Exception exception)
        {
            Debug.LogError($"[SaveManager] Failed to save data.\n{exception}");
        }
    }
}
