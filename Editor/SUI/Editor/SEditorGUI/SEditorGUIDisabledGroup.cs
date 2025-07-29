using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUIDisabledGroup : SUIElement
    {
        private SUIElement content;
        private bool disabled;
        private Func<bool> where = () => true;

        public SEditorGUIDisabledGroup()
        {

        }

        public SEditorGUIDisabledGroup(bool disabled)
        {
            this.disabled = disabled;
        }

        public SEditorGUIDisabledGroup Content(SUIElement content = null)
        {
            this.content = content;
            return this;
        }

        public SEditorGUIDisabledGroup Where(Func<bool> callback)
        {
            this.where = callback;
            return this;
        }

        public void Render()
        {
            EditorGUI.BeginDisabledGroup(disabled);
            if (where())
                content?.Render();
            EditorGUI.EndDisabledGroup();
        }
    }
}