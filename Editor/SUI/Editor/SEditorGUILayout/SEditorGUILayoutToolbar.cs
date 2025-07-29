using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutToolbar : SUIElement
    {
        private string[] labels;
        private int selected;
        private UnityAction<int> onValueChanged;

        public SEditorGUILayoutToolbar(int selected, params string[] labels)
        {
            this.selected = selected;
            this.labels = labels;
        }

        public SEditorGUILayoutToolbar OnValueChanged(UnityAction<int> onValueChanged)
        {
            this.onValueChanged = onValueChanged;
            return this;
        }

        public override void Render()
        {
            int newIndex = GUILayout.Toolbar(selected, labels);
            if (newIndex != selected)
            {
                selected = newIndex;
                onValueChanged?.Invoke(newIndex);
            }
        }
    }
}
