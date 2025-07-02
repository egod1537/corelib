using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutSlider : SUIElement
    {
        private float value;
        private float minValue;
        private float maxValue;
        private UnityAction<float> onValueChanged;

        public SEditorGUILayoutSlider(float value, float minValue, float maxValue)
        {
            this.value = value;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public SEditorGUILayoutSlider OnValueChanged(UnityAction<float> onValueChanged)
        {
            this.onValueChanged = onValueChanged;
            return this;
        }

        public override void Render()
        {
            float newValue = EditorGUILayout.Slider(value, minValue, maxValue);
            if (newValue != value)
            {
                value = newValue;
                onValueChanged?.Invoke(newValue);
            }
        }
    }
}