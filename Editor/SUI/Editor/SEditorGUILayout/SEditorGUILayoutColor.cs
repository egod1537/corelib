using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutColor : SUIElement
    {
        private string prefix;
        private Color value;
        private float? width;
        private float? height;
        private UnityAction<Color> onValueChanged;

        public SEditorGUILayoutColor(string prefix, Color value)
        {
            this.prefix = prefix;
            this.value = value;
        }

        public SEditorGUILayoutColor OnValueChanged(UnityAction<Color> onValueChanged)
        {
            this.onValueChanged = onValueChanged;
            return this;
        }

        public SEditorGUILayoutColor Width(float width)
        {
            this.width = width;
            return this;
        }

        public SEditorGUILayoutColor Height(float height)
        {
            this.height = height;
            return this;
        }

        public override void Render()
        {
            List<GUILayoutOption> options = new();
            if (width != null) options.Add(GUILayout.Width(width.Value));
            if (height != null) options.Add(GUILayout.Height(height.Value));

            Color newValue = EditorGUILayout.ColorField(prefix, value, options.ToArray());

            if (newValue != value)
            {
                value = newValue;
                onValueChanged?.Invoke(newValue);
            }
        }
    }
}