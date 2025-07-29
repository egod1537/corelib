using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutSlider : SUIElement
    {
        private string preifx;
        private float value;
        private float minValue;
        private float maxValue;
        private UnityAction<float> onValueChanged;

        public SEditorGUILayoutSlider(string preifx, float value, float minValue, float maxValue)
        {
            this.preifx = preifx;
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
            float newValue = EditorGUILayout.Slider(preifx, value, minValue, maxValue);
            if (newValue != value)
            {
                value = newValue;
                onValueChanged?.Invoke(newValue);
            }
        }
    }
}