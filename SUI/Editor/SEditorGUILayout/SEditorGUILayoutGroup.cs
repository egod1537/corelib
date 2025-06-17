using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutGroup : SUIElement
    {
        private SUIElement content;
        private string title = "";

        public SEditorGUILayoutGroup(string title)
        {
            this.title = title;
        }

        public SEditorGUILayoutGroup Content(SUIElement content = null)
        {
            this.content = content;
            return this;
        }

        public override void Render()
        {
            SEditorGUILayout.Vertical("box")
            .Content(
                SEditorGUILayout.Vertical("HelpBox")
                .Content(
                    SEditorGUILayout.Label($"[{title}]")
                    .Bold()
                    .Align(TextAnchor.MiddleCenter)
                )
                + SEditorGUILayout.Separator()
                + SEditorGUILayout.Action(() => content?.Render())
            )
            .Render();
        }
    }
}