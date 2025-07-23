#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
public static class InspectorDeselector {
    static InspectorDeselector() {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state) {
        if (state == PlayModeStateChange.ExitingPlayMode) {
            Selection.activeObject = null;
            EditorApplication.update += CloseInspectorAfterDelay;
        }
    }

    private static void CloseInspectorAfterDelay() {
        EditorApplication.update -= CloseInspectorAfterDelay;
        Selection.activeObject = null;
    }
}
#endif