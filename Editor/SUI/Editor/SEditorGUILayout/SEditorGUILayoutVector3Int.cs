using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutVector3Int : SUIElement
    {
        private string prefix;
        private Vector3Int value;
        private UnityAction<Vector3Int> onValueChanged;

        public SEditorGUILayoutVector3Int(string prefix, Vector3Int value)
        {
            this.prefix = prefix;
            this.value = value;
        }

        public SEditorGUILayoutVector3Int OnValueChanged(UnityAction<Vector3Int> onValueChanged)
        {
            this.onValueChanged = onValueChanged;
            return this;
        }

        public override void Render()
        {
            Vector3Int newValue = EditorGUILayout.Vector3IntField(prefix, value);
            if (newValue != value)
            {
                value = newValue;
                onValueChanged?.Invoke(newValue);
            }
        }
    }
}