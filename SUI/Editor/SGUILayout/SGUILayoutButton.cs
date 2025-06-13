using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SGUILayoutButton : SUIElement
    {
        private readonly string label;
        private UnityAction onClick;
        private GUIStyle guiStyle;

        public SGUILayoutButton(string label)
        {
            this.label = label;
        }

        public SGUILayoutButton OnClick(UnityAction onClick)
        {
            this.onClick = onClick;
            return this;
        }

        public SGUILayoutButton GUIStyle(GUIStyle guiStyle)
        {
            this.guiStyle = guiStyle;
            return this;
        }

        public override void Render()
        {
            if (GUILayout.Button(label, guiStyle))
                onClick?.Invoke();
        }
    }
}