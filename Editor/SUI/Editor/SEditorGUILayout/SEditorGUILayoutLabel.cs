using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutLabel : SUIElement
    {
        private string label;
        private float? width;
        private bool isBold = false;
        private TextAnchor? textAnchor;
        private UnityAction<int> onValueChanged;

        public SEditorGUILayoutLabel(string label)
        {
            this.label = label;
        }

        public SEditorGUILayoutLabel Width(float width)
        {
            this.width = width;
            return this;
        }

        public SEditorGUILayoutLabel Bold()
        {
            this.isBold = true;
            return this;
        }

        public SEditorGUILayoutLabel Align(TextAnchor textAnchor)
        {
            this.textAnchor = textAnchor;
            return this;
        }

        public override void Render()
        {
            List<GUILayoutOption> options = new();
            if (width != null) options.Add(GUILayout.Width(width.Value));

            GUIStyle guiStyle = new GUIStyle(EditorStyles.label);
            if (isBold) guiStyle.fontStyle = FontStyle.Bold;
            if (textAnchor != null) guiStyle.alignment = textAnchor.Value;

            EditorGUILayout.LabelField(label, guiStyle, options.ToArray());
        }
    }
}