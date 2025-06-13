using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutHorizontal : SUIElement
    {
        private SUIElement content;
        private string style = "";


        public SEditorGUILayoutHorizontal()
        {

        }

        public SEditorGUILayoutHorizontal(string style)
        {
            this.style = style;
        }

        public SEditorGUILayoutHorizontal Content(SUIElement content = null)
        {
            this.content = content;
            return this;
        }

        public SEditorGUILayoutHorizontal Style(string style)
        {
            this.style = style;
            return this;
        }

        public override void Render()
        {
            EditorGUILayout.BeginHorizontal(style);
            content?.Render();
            EditorGUILayout.EndHorizontal();
        }
    }
}