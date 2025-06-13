using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UObject = UnityEngine.Object;

namespace Corelib.SUI
{
    public class SEditorGUILayoutObject : SUIElement
    {
        private string label;
        private UObject value;
        private Type type;
        private UnityAction<UObject> onValueChanged;

        public SEditorGUILayoutObject(string label, UObject value, Type type)
        {
            this.label = label;
            this.value = value;
            this.type = type;
        }

        public SEditorGUILayoutObject OnValueChanged(UnityAction<UObject> onValueChanged)
        {
            this.onValueChanged = onValueChanged;
            return this;
        }

        public override void Render()
        {
            UObject newValue = EditorGUILayout.ObjectField(label, value, type, false);
            if (newValue != value)
            {
                value = newValue;
                onValueChanged?.Invoke(newValue);
            }
        }
    }
}