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

        public override void Render()
        {
            Enum newValue = EditorGUILayout.EnumPopup(label, value);
            if (newValue != value)
            {
                value = newValue;
                onValueChanged?.Invoke(newValue);
            }
        }
    }
}