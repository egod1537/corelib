using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUIAutoRect : SUIElement
    {
        private readonly string label;
        private UnityAction onClick;

        public SEditorGUIAutoRect(string label)
        {
            this.label = label;
        }

        public SEditorGUIAutoRect OnClick(UnityAction onClick)
        {
            this.onClick = onClick;
            return this;
        }

        public override void Render()
        {
            if (GUILayout.Button(label))
                onClick?.Invoke();
        }
    }
}