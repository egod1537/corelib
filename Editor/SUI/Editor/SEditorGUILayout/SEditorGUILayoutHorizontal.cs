using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutHorizontal : SUIElement
    {
        private SUIElement content;
        private string style = "";
        private float? labelWidth;


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

        public SEditorGUILayoutHorizontal LabelWidth(float labelWidth)
        {
            this.labelWidth = labelWidth;
            return this;
        }

        public override void Render()
        {
            EditorGUILayout.BeginHorizontal(string.IsNullOrWhiteSpace(style) ? GUIStyle.none : style);
            var originalLabelWidth = EditorGUIUtility.labelWidth;
            if (labelWidth != null) EditorGUIUtility.labelWidth = labelWidth.Value;
            content?.Render();
            if (labelWidth != null) EditorGUIUtility.labelWidth = originalLabelWidth;
            EditorGUILayout.EndHorizontal();
        }
    }
}