using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SGUILayoutSelectionGrid : SUIElement
    {
        private int value;
        private UnityAction<int> onValueChanged;
        private GUIStyle guiStyle;

        private List<GUIContent> guiContents;
        private int columnCount;

        public SGUILayoutSelectionGrid(int value, List<GUIContent> guiContents, int columnCount)
        {
            this.value = value;
            this.guiContents = guiContents;
            this.columnCount = columnCount;
        }

        public SGUILayoutSelectionGrid OnValueChanged(UnityAction<int> onValueChanged)
        {
            this.onValueChanged = onValueChanged;
            return this;
        }

        public SGUILayoutSelectionGrid GUIStyle(GUIStyle guiStyle)
        {
            this.guiStyle = guiStyle;
            return this;
        }

        public override void Render()
        {
            int newValue = GUILayout.SelectionGrid(value, guiContents.ToArray(), columnCount);

            if (newValue != value)
            {
                onValueChanged?.Invoke(newValue);
                value = newValue;
            }
        }
    }
}