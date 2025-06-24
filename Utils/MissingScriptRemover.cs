using UnityEditor;
using UnityEngine;

namespace Corelib.Utils
{
    public static class MissingScriptRemover
    {
        [MenuItem("Tools/Cleanup/Remove Missing Scripts In Scene")]
        public static void RemoveMissingScriptsInScene()
        {
            int count = 0;
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);

            foreach (GameObject go in allObjects)
            {
                int removed = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
                if (removed > 0)
                {
                    Debug.Log($"Removed {removed} missing script(s) from: {go.name}", go);
                    count += removed;
                }
            }

            Debug.Log($"✅ Done. Removed total {count} missing script(s) in scene.");
        }

        [MenuItem("Tools/Cleanup/Remove Missing Scripts In Project Prefabs")]
        public static void RemoveMissingScriptsInPrefabs()
        {
            string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
            int totalRemoved = 0;

            foreach (string guid in prefabGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (prefab == null) continue;

                GameObject instance = PrefabUtility.LoadPrefabContents(path);
                int removed = 0;

                foreach (Transform tr in instance.GetComponentsInChildren<Transform>(true))
                {
                    removed += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(tr.gameObject);
                }

                if (removed > 0)
                {
                    PrefabUtility.SaveAsPrefabAsset(instance, path);
                    Debug.Log($"Cleaned {removed} missing script(s) in prefab: {path}");
                    totalRemoved += removed;
                }

                PrefabUtility.UnloadPrefabContents(instance);
            }

            Debug.Log($"✅ Done. Removed total {totalRemoved} missing script(s) in all prefabs.");
        }
    }

}