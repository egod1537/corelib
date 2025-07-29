using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutFoldout : SUIElement
    {
        private string label;
        private bool value;
        private SUIElement content;
        private int indentLevel = 0;
        private UnityAction<bool> onValueChanged;

        public SEditorGUILayoutFoldout(string label, bool value)
        {
            this.label = label;
            this.value = value;
        }

        public SEditorGUILayoutFoldout OnValueChanged(UnityAction<bool> onValueChanged)
        {
            this.onValueChanged = onValueChanged;
            return this;
        }

        public SEditorGUILayoutFoldout Content(SUIElement content = null)
        {
            this.content = content;
            return this;
        }

        public override void Render()
        {
            bool newValue = EditorGUILayout.Foldout(value, label);
            if (newValue != value)
            {
                value = newValue;
                onValueChanged?.Invoke(newValue);
            }
            if (newValue)
            {
                if (content != null) content.Render();
            }
        }
    }
}