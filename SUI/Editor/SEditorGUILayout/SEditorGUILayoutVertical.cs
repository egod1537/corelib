using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutVertical : SUIElement
    {
        private SUIElement content;
        private string style = "";

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

        public override void Render()
        {
            EditorGUILayout.BeginVertical(style);
            content?.Render();
            EditorGUILayout.EndVertical();
        }
    }
}