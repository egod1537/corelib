using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UObject = UnityEngine.Object;

namespace Corelib.SUI
{
    public class SEditorGUILayoutEnum : SUIElement
    {
        private string label;
        private Enum value;
        private UnityAction<Enum> onValueChanged;
        private float? width;

        public SEditorGUILayoutEnum(string label, Enum value)
        {
            this.label = label;
            this.value = value;
        }

        public SEditorGUILayoutEnum OnValueChanged(UnityAction<Enum> onValueChanged)
        {
            this.onValueChanged = onValueChanged;
            return this;
        }

        public SEditorGUILayoutEnum Width(float width)
        {
            this.width = width;
            return this;
        }

        public override void Render()
        {
            List<GUILayoutOption> options = new();
            if (width != null) options.Add(GUILayout.Width(width.Value));

            Enum newValue = EditorGUILayout.EnumPopup(label, value, options.ToArray());
            if (newValue != value)
            {
                value = newValue;
                onValueChanged?.Invoke(newValue);
            }
        }
    }
}