
using Ingame;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Corelib.Utils
{
    [InitializeOnLoad]
    public static class HierarchyExtender
    {
        private static Rect rect = new Rect();
        private static Rect selectionRect = new Rect();

        static HierarchyExtender()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            HierarchyExtender.selectionRect = selectionRect;
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
            ExtendUIComponentBehaviour(gameObject);
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

        static void ExtendUIComponentBehaviour(GameObject gameObject)
        {
            int prevDepth = GUI.depth;
            GUI.depth = -100;
            if (gameObject.GetComponent<UIComponentBahaviour>() != null)
            {
                float width = 48f;
                Rect newRect = new Rect(rect)
                {
                    x = 200f,
                    height = 2f,
                    width = 100f,
                };
                EditorGUI.DrawRect(newRect, Color.green);
            }
            GUI.depth = prevDepth;
        }
    }
}
