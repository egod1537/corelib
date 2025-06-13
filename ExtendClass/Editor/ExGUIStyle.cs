using UnityEditor;
using UnityEngine;
namespace Corelib.Utils
{
    public static class ExGUIStyle
    {
        public static GUIStyle Label(this GUIStyle style)
            => new GUIStyle(GUI.skin.label);
        public static GUIStyle FoldOut(this GUIStyle style)
            => new GUIStyle(EditorStyles.foldout);
        public static GUIStyle Box(this GUIStyle style)
            => new GUIStyle(GUI.skin.box);
        public static GUIStyle Button(this GUIStyle style)
            => new GUIStyle(GUI.skin.button);
        public static GUIStyle Popup(this GUIStyle style)
            => new GUIStyle(EditorStyles.popup);
        public static GUIStyle Window(this GUIStyle style)
            => new GUIStyle(GUI.skin.window);
        public static GUIStyle TextField(this GUIStyle style)
            => new GUIStyle(GUI.skin.textField);

        public static GUIStyle SetTextColor(this GUIStyle style, Color color)
        {
            style.normal.textColor = color;
            return style;
        }

        public static GUIStyle SetBold(this GUIStyle style, bool flag)
        {
            style.fontStyle = flag ? FontStyle.Bold : FontStyle.Normal;
            return style;
        }

        public static GUIStyle SetFixedHeight(this GUIStyle style, float height)
        {
            style.fixedHeight = height;
            return style;
        }

        public static GUIStyle SetAlign(this GUIStyle style, TextAnchor anchor)
        {
            style.alignment = anchor;
            return style;
        }
    }
}