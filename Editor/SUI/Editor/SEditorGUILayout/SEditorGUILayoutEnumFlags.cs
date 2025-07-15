using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UObject = UnityEngine.Object;

namespace Corelib.SUI
{
    public class SEditorGUILayoutEnumFlags : SUIElement
    {
        private string label;
        private Enum value;
        private UnityAction<Enum> onValueChanged;

        public SEditorGUILayoutEnumFlags(string label, Enum value)
        {
            this.label = label;
            this.value = value;
        }

        public SEditorGUILayoutEnumFlags OnValueChanged(UnityAction<Enum> onValueChanged)
        {
            this.onValueChanged = onValueChanged;
            return this;
        }

        public override void Render()
        {
            Enum newValue = EditorGUILayout.EnumFlagsField(label, value);
            if (newValue != value)
            {
                value = newValue;
                onValueChanged?.Invoke(newValue);
            }
        }
    }
}