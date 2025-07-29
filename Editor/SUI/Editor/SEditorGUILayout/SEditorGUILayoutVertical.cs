using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutVertical : SUIElement
    {
        private SUIElement content;
        private string style = "";
        private GUIStyle guiStyle;
        private Func<bool> where = () => true;

        public SEditorGUILayoutVertical()
        {

        }

        public SEditorGUILayoutVertical(string style)
        {
            this.style = style;
        }

        public SEditorGUILayoutVertical(GUIStyle guiStyle)
        {
            this.guiStyle = guiStyle;
        }

        public SEditorGUILayoutVertical Content(SUIElement content = null)
        {
            this.content = content;
            return this;
        }

        public SEditorGUILayoutVertical Style(string style)
        {
            this.style = style;
            return this;
        }

        public SEditorGUILayoutVertical Where(Func<bool> callback)
        {
            this.where = callback;
            return this;
        }

        public override void Render()
        {
            if (guiStyle != null)
                EditorGUILayout.BeginVertical(guiStyle);
            else
                EditorGUILayout.BeginVertical(string.IsNullOrWhiteSpace(style) ? GUIStyle.none : style);

            if (where())
                content?.Render();
            EditorGUILayout.EndVertical();
        }
    }
}