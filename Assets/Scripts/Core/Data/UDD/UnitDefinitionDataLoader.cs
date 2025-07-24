using UnityEngine;
using System.IO;

public class UDDLoader : MonoBehaviour {
    public string JsonFileName;
    public UnitDefinitionData LoadedUDD;

    public static UnitDefinitionData ReadJSON(string jsonFileName) {
        string path = Path.Combine(Application.dataPath, "Resources/JSON/UDD", jsonFileName);
        if (!File.Exists(path)) {
            Debug.LogError($"UDD file not found: {path}");
            return null;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<UnitDefinitionData>(json);
    }
    
    [ContextMenu("Load UDD From JSON")]
    public void LoadJson() {
        LoadedUDD = ReadJSON(JsonFileName);
        Debug.Log($"Loaded {JsonFileName} successfully.");
    }
    
    [ContextMenu("Save UDD To JSON")]
    public void SaveJson() {
        if (LoadedUDD == null) {
            Debug.LogError("No UDD loaded to save.");
            return;
        }

        string path = Path.Combine(Application.dataPath, "Resources/JSON/UDD", JsonFileName);
        string json = JsonUtility.ToJson(LoadedUDD, true);
        File.WriteAllText(path, json);
        Debug.Log($"Saved to {path}");
    }
}