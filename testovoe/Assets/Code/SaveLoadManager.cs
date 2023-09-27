using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float playerHealth;
    public Vector3 position;
    public int currentAmmo;
    public bool isdead;
}

public static class SaveLoadManager
{
    public static void SavePlayerData(PlayerData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        string savePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        File.WriteAllText(savePath, jsonData);
    }

    public static PlayerData LoadPlayerData()
    {
        string loadPath = Path.Combine(Application.persistentDataPath, "playerData.json");

        if (File.Exists(loadPath))
        {
            string jsonData = File.ReadAllText(loadPath);
            return JsonUtility.FromJson<PlayerData>(jsonData);
        }

        return null; // ¬озвращаем null, если нет сохраненных данных
    }
}