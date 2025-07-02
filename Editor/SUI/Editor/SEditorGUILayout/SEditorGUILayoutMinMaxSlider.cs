using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutMinMaxSlider : SUIElement
    {
        private float minValue;
        private float maxValue;

        private float? width;
        private float? height;

        private float limitMin = 0f;
        private float limitMax = 1f;

        private UnityAction<float, float> onValueChanged;

        public SEditorGUILayoutMinMaxSlider(float minValue, float maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public SEditorGUILayoutMinMaxSlider Range(float limitMin, float limitMax)
        {
            this.limitMin = limitMin;
            this.limitMax = limitMax;
            return this;
        }

        public SEditorGUILayoutMinMaxSlider OnValueChanged(UnityAction<float, float> callback)
        {
            this.onValueChanged = callback;
            return this;
        }

        public SEditorGUILayoutMinMaxSlider Width(float width)
        {
            this.width = width;
            return this;
        }

        public SEditorGUILayoutMinMaxSlider Height(float height)
        {
            this.height = height;
            return this;
        }

        public override void Render()
        {
            List<GUILayoutOption> options = new();
            if (width != null) options.Add(GUILayout.Width(width.Value));
            if (height != null) options.Add(GUILayout.Height(height.Value));

            float newMin = minValue;
            float newMax = maxValue;

            EditorGUILayout.MinMaxSlider(ref newMin, ref newMax, limitMin, limitMax, options.ToArray());

            newMin = Mathf.Max(newMin, limitMin);
            newMax = Math.Min(newMax, limitMax);

            if (!Mathf.Approximately(newMin, minValue) || !Mathf.Approximately(newMax, maxValue))
            {
                minValue = newMin;
                maxValue = newMax;
                onValueChanged?.Invoke(newMin, newMax);
            }
        }
    }
}