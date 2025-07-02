using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutVector3 : SUIElement
    {
        private string prefix;
        private Vector3 value;
        private UnityAction<Vector3> onValueChanged;

        public SEditorGUILayoutVector3(string prefix, Vector3 value)
        {
            this.prefix = prefix;
            this.value = value;
        }

        public SEditorGUILayoutVector3 OnValueChanged(UnityAction<Vector3> onValueChanged)
        {
            this.onValueChanged = onValueChanged;
            return this;
        }

        public override void Render()
        {
            Vector3 newValue = EditorGUILayout.Vector3Field(prefix, value);
            if (newValue != value)
            {
                value = newValue;
                onValueChanged?.Invoke(newValue);
            }
        }
    }
}