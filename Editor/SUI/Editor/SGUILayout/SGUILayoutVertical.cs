using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SGUILayoutVertical : SUIElement
    {
        private SUIElement content;
        private string style = "";

        public SGUILayoutVertical()
        {

        }

        public SGUILayoutVertical(SUIElement content)
        {
            this.content = content;
        }

        public SGUILayoutVertical Style(string style)
        {
            this.style = style;
            return this;
        }

        public override void Render()
        {
            GUILayout.BeginVertical(style);
            content?.Render();
            GUILayout.EndVertical();
        }
    }
}