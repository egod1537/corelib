using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutButton : SUIElement, IWidthable<SEditorGUILayoutButton>
    {
        private readonly string label;
        private readonly GUIContent guiContent;
        private UnityAction onClick;
        private float? width;
        private Func<bool> where = () => true;

        public SEditorGUILayoutButton(string label)
        {
            this.label = label;
        }

        public SEditorGUILayoutButton(GUIContent guiContent)
        {
            this.guiContent = guiContent;
        }

        public SEditorGUILayoutButton Where(Func<bool> where)
        {
            this.where = where;
            return this;
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
            if (!where()) return;
            var options = new List<GUILayoutOption>();

            if (width != null)
            {
                options.Add(GUILayout.Width(width.Value));
            }

            if (guiContent != null)
            {
                if (GUILayout.Button(guiContent, options.ToArray()))
                {
                    onClick?.Invoke();
                }
            }
            else
            {
                if (GUILayout.Button(label, options.ToArray()))
                {
                    onClick?.Invoke();
                }
            }

        }
    }
}