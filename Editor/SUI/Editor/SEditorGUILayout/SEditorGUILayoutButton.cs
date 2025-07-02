using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutButton : SUIElement
    {
        private readonly string label;
        private UnityAction onClick;

        public SEditorGUILayoutButton(string label)
        {
            this.label = label;
        }

        public SEditorGUILayoutButton OnClick(UnityAction onClick)
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