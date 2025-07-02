using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutToggle : SUIElement
    {
        private string label;
        private bool value;
        private float? width;
        private float? height;
        private UnityAction<bool> onValueChanged;

        public SEditorGUILayoutToggle(string label, bool value)
        {
            this.label = label;
            this.value = value;
        }

        public SEditorGUILayoutToggle OnValueChanged(UnityAction<bool> onValueChanged)
        {
            this.onValueChanged = onValueChanged;
            return this;
        }

        public SEditorGUILayoutToggle Width(float width)
        {
            this.width = width;
            return this;
        }

        public SEditorGUILayoutToggle Height(float height)
        {
            this.height = height;
            return this;
        }

        public override void Render()
        {
            List<GUILayoutOption> options = new();
            if (width != null) options.Add(GUILayout.Width(width.Value));
            if (height != null) options.Add(GUILayout.Height(height.Value));

            bool newValue = EditorGUILayout.Toggle(label, value, options.ToArray());
            if (newValue != value)
            {
                value = newValue;
                onValueChanged?.Invoke(newValue);
            }
        }
    }
}