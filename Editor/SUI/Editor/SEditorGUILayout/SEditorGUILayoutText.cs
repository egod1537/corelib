using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutText : SUIElement
    {
        private string prefix;
        private string value;
        private float? width;
        private float? height;
        private UnityAction<string> onValueChanged;

        public SEditorGUILayoutText(string prefix, string value)
        {
            this.prefix = prefix;
            this.value = value;
        }

        public SEditorGUILayoutText OnValueChanged(UnityAction<string> onValueChanged)
        {
            this.onValueChanged = onValueChanged;
            return this;
        }

        public SEditorGUILayoutText Width(float width)
        {
            this.width = width;
            return this;
        }

        public SEditorGUILayoutText Height(float height)
        {
            this.height = height;
            return this;
        }

        public override void Render()
        {
            List<GUILayoutOption> options = new();
            if (width != null) options.Add(GUILayout.Width(width.Value));
            if (height != null) options.Add(GUILayout.Height(height.Value));

            string newValue = EditorGUILayout.TextField(prefix, value, options.ToArray());

            if (newValue != value)
            {
                value = newValue;
                onValueChanged?.Invoke(newValue);
            }
        }
    }
}