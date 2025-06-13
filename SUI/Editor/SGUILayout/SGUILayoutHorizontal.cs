using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SGUILayoutHorizontal : SUIElement
    {
        private readonly SUIElement content;
        private string style = "";

        public SGUILayoutHorizontal()
        {

        }

        public SGUILayoutHorizontal(SUIElement content)
        {
            this.content = content;
        }

        public SGUILayoutHorizontal Style(string style)
        {
            this.style = style;
            return this;
        }

        public override void Render()
        {
            GUILayout.BeginHorizontal(style);
            content?.Render();
            GUILayout.EndHorizontal();
        }
    }
}