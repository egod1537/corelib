using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

namespace Corelib.Utils
{
    public static class EditorGUIDrawer
    {
        public static void DrawInspectorLine()
        {
            EditorGUILayout.Space();
            var lineRect = GUILayoutUtility.GetRect(GUIContent.none,
                                                     GUIStyle.none,
                                                     GUILayout.Height(2));
            lineRect.y -= 3;
            lineRect.width += 20;
            Handles.color = Color.black;

            var start = new Vector2(0, lineRect.y);
            var end = new Vector2(lineRect.width, lineRect.y);
            Handles.DrawLine(start, end);
        }

        public static void DrawInspectorTitlebar(Editor editor, bool foldOut, UnityAction<bool> onFoldOut)
        {
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none,
                                                GUIStyle.none,
                                                GUILayout.Height(20));
            rect.x = 0;
            rect.y -= 5;
            rect.width += 20;
            onFoldOut.Invoke(EditorGUI.InspectorTitlebar(rect,
                                                foldOut,
                                                editor.target,
                                                true));
        }

        public class ContainerBuilder
        {
            private UnityEvent onOpen = new();
            private UnityEvent onClose = new();

            public ContainerBuilder GUILayoutCenterAlign()
            {
                onOpen.AddListener(() =>
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                });
                onClose.AddListener(() =>
                {
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                });
                return this;
            }

            public ContainerBuilder HandleArea(Rect rect)
            {
                onOpen.AddListener(() =>
                {
                    Handles.BeginGUI();
                    GUILayout.BeginArea(rect);
                });
                onClose.AddListener(() =>
                {
                    GUILayout.EndArea();
                    Handles.EndGUI();
                });
                return this;
            }

            public void Draw(UnityAction action)
            {
                onOpen.Invoke();
                action();
                onClose.Invoke();
            }
        }

        public static ContainerBuilder InContainer()
            => new ContainerBuilder();
    }
}