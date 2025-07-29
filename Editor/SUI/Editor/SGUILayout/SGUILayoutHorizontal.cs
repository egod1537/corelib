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

        public void Render()
        {
            GUILayout.BeginHorizontal(string.IsNullOrWhiteSpace(style) ? GUIStyle.none : style);
            content?.Render();
            GUILayout.EndHorizontal();
        }
    }
}