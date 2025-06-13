
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Corelib.Utils
{
    [InitializeOnLoad]
    public static class HierarchyButtonExtender
    {
        private static Rect rect = new Rect();

        static HierarchyButtonExtender()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            rect = new Rect(selectionRect.xMax, selectionRect.y, 0, selectionRect.height);

            Object obj = EditorUtility.InstanceIDToObject(instanceID);

            ExtendGameobject(obj);
        }

        static void ExtendGameobject(Object obj)
        {
            if (obj is not GameObject) return;
            GameObject gameObject = obj as GameObject;

            ExtendRectTrasnform(gameObject);
            ExtendGUILayoutGroup(gameObject);
        }

        static void ExtendRectTrasnform(GameObject gameObject)
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform == null) return;

            // float width = 16f;
            // rect = new Rect(rect)
            // {
            //     x = rect.x - width,
            //     width = width,
            // };

            // if (GUI.Button(rect, "+"))
            // {
            //     GameObject go = new GameObject("Child");
            //     if (Selection.activeTransform != null)
            //         go.transform.SetParent(Selection.activeTransform, false);
            // }
        }

        static void ExtendGUILayoutGroup(GameObject gameObject)
        {
            GridLayoutGroup gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
            if (gridLayoutGroup == null) return;

            float width = 48f;
            rect = new Rect(rect)
            {
                x = rect.x - width,
                width = width,
            };

            if (GUI.Button(rect, "index"))
            {
                Transform tr = gameObject.transform;
                for (int i = 0; i < tr.childCount; i++)
                {
                    Transform child = tr.GetChild(i);
                    child.name = $"{i}";
                }
            }
        }
    }
}
