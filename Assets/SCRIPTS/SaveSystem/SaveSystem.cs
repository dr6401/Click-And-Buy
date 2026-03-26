using System.IO;
using UnityEngine;

public class SaveSystem
{
    private static string path = Path.Combine(Application.persistentDataPath, "save.json");

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        //Debug.Log(json);
        File.WriteAllText(path, json);
        Debug.Log("SAVED JSON DATA");
    }

    public static SaveData Load()
    {
        Debug.Log($"path = {path}");
        if (!File.Exists(path) || string.IsNullOrWhiteSpace(File.ReadAllText(path)) || string.IsNullOrEmpty(File.ReadAllText(path)))
        {
            Debug.Log("Data JSON did not exist, creating new one");
            SaveData newData = new SaveData();
            Save(newData);
            return newData;
        }

        string json = File.ReadAllText(path);
        Debug.Log("Data JSON did exist, loading it and saving");
        //Debug.Log("Loaded JSON: " + json);
        return JsonUtility.FromJson<SaveData>(json);
    }
}
