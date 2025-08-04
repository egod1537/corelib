using UnityEngine;
using UnityEditor;

namespace Corelib.Utils
{
    public class RectTransformScaler : EditorWindow
    {
        private float scaleFactor = 1.0f;

        [MenuItem("Tools/Scale RectTransforms")]
        private static void ShowWindow()
        {
            GetWindow<RectTransformScaler>("Scale RectTransforms");
        }

        private void OnGUI()
        {
            GUILayout.Label("Scale All RectTransforms", EditorStyles.boldLabel);
            scaleFactor = EditorGUILayout.FloatField("Scale Factor", scaleFactor);

            if (GUILayout.Button("Scale Selected GameObject"))
            {
                if (Selection.activeGameObject == null)
                {
                    Debug.LogWarning("No GameObject selected.");
                    return;
                }

                RectTransform root = Selection.activeGameObject.GetComponent<RectTransform>();
                if (root == null)
                {
                    Debug.LogWarning("Selected object has no RectTransform.");
                    return;
                }

                Undo.RecordObject(root, "Scale RectTransforms");
                ScaleRectRecursive(root);
                Debug.Log("Scaling complete.");
            }
        }

        private void ScaleRectRecursive(RectTransform rect)
        {
            Vector2 size = rect.sizeDelta;
            size *= scaleFactor;
            rect.sizeDelta = size;

            foreach (Transform child in rect)
            {
                if (child is RectTransform childRect)
                    ScaleRectRecursive(childRect);
            }
        }
    }

}