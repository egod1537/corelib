using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutButton : SUIElement, IWidthable<SEditorGUILayoutButton>
    {
        private readonly string label;
        private UnityAction onClick;
        private float? width;

        public SEditorGUILayoutButton(string label)
        {
            this.label = label;
        }

        public SEditorGUILayoutButton OnClick(UnityAction onClick)
        {
            this.onClick = onClick;
            return this;
        }

        public SEditorGUILayoutButton Width(float width)
        {
            this.width = width;
            return this;
        }

        public override void Render()
        {
            var options = new List<GUILayoutOption>();

            if (width != null)
            {
                options.Add(GUILayout.Width(width.Value));
            }

            if (GUILayout.Button(label, options.ToArray()))
            {
                onClick?.Invoke();
            }
        }
    }
}