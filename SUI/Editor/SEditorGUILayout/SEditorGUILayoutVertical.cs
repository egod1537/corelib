using System;
using UnityEditor;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutVertical : SUIElement
    {
        private SUIElement content;
        private string style = "";
        private Func<bool> where = () => true;

        public SEditorGUILayoutVertical()
        {

        }

        public SEditorGUILayoutVertical(string style)
        {
            this.style = style;
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
            EditorGUILayout.BeginVertical(style);
            if (where())
                content?.Render();
            EditorGUILayout.EndVertical();
        }
    }
}