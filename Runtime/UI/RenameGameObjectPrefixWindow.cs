using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class RenameGameObjectPrefixWindow : EditorWindow
{
    private string oldPrefix = "<ui>";
    private string newPrefix = "<ref>";

    [MenuItem("Tools/Refactor/Rename GameObject Prefix")]
    public static void ShowWindow()
    {
        var window = GetWindow<RenameGameObjectPrefixWindow>("Rename Prefix");
        window.minSize = new Vector2(300, 120);
    }

    private void OnGUI()
    {
        GUILayout.Label("Rename GameObject Prefix", EditorStyles.boldLabel);

        oldPrefix = EditorGUILayout.TextField("Old Prefix", oldPrefix);
        newPrefix = EditorGUILayout.TextField("New Prefix", newPrefix);

        GUILayout.Space(10);

        if (GUILayout.Button("Rename in Scene + Prefabs"))
        {
            if (string.IsNullOrEmpty(oldPrefix) || string.IsNullOrEmpty(newPrefix))
            {
                EditorUtility.DisplayDialog("Error", "Prefix fields cannot be empty.", "OK");
                return;
            }

            int count = RenameInHierarchyAndPrefabs(oldPrefix, newPrefix);
            EditorUtility.DisplayDialog("Done", $"Renamed {count} GameObjects.", "OK");
        }
    }

    private int RenameInHierarchyAndPrefabs(string oldTag, string newTag)
    {
        int renameCount = 0;

        // Hierarchy
        foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (!IsInEditableScene(go)) continue;
            if (go.name.StartsWith(oldTag))
            {
                Undo.RecordObject(go, "Rename GameObject");
                go.name = go.name.Replace(oldTag, newTag);
                EditorUtility.SetDirty(go);
                renameCount++;
            }
        }

        // Prefabs
        var guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            bool modified = false;
            foreach (var child in prefab.GetComponentsInChildren<Transform>(true))
            {
                if (child.name.StartsWith(oldTag))
                {
                    Undo.RecordObject(child.gameObject, "Rename Prefab GameObject");
                    child.name = child.name.Replace(oldTag, newTag);
                    EditorUtility.SetDirty(child.gameObject);
                    modified = true;
                    renameCount++;
                }
            }

            if (modified)
            {
                AssetDatabase.SaveAssets();
            }
        }

        return renameCount;
    }

    private bool IsInEditableScene(GameObject go)
    {
        return !EditorUtility.IsPersistent(go) &&
               go.hideFlags == HideFlags.None &&
               !string.IsNullOrEmpty(go.scene.name);
    }
}
