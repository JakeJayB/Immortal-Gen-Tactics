using UnityEngine;
using System.IO;

public class UDDLoader : MonoBehaviour {
    public string JsonFileName;

    [TextArea(10, 30)]
    public string previewJson;
    
    public UnitDefinitionData LoadedUDD;
    
    [ContextMenu("Load UDD From JSON")]
    public void LoadJson() {
        string path = Path.Combine(Application.dataPath, "Resources/JSON/UDD", JsonFileName);
        if (!File.Exists(path)) {
            Debug.LogError($"UDD file not found: {path}");
            return;
        }

        string json = File.ReadAllText(path);
        LoadedUDD = JsonUtility.FromJson<UnitDefinitionData>(json);
        previewJson = json;
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
        previewJson = json;
        Debug.Log($"Saved to {path}");
    }
}