using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutFoldGroup : SUIElement
    {
        private string label;
        private bool value;
        private SUIElement content;
        private SUIElement headerRight;
        private UnityAction<bool> onValueChanged;

        public SEditorGUILayoutFoldGroup(string label, bool value)
        {
            this.label = label;
            this.value = value;
        }

        public SEditorGUILayoutFoldGroup OnValueChanged(UnityAction<bool> onValueChanged)
        {
            this.onValueChanged = onValueChanged;
            return this;
        }

        public SEditorGUILayoutFoldGroup Content(SUIElement content = null)
        {
            this.content = content;
            return this;
        }

        public SEditorGUILayoutFoldGroup HeaderRight(SUIElement headerRight)
        {
            this.headerRight = headerRight;
            return this;
        }

        public override void Render()
        {
            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Vertical("helpbox")
                .Content(
                    SEditorGUILayout.Horizontal()
                    .Content(
                        SGUILayout.Space(15)
                        + SEditorGUILayout.FoldoutHeaderGroup(label, value)
                        .OnValueChanged(value =>
                        {
                            this.value = value;
                            onValueChanged?.Invoke(value);
                        })
                        + (headerRight ?? SUIElement.Empty())
                    )
                )
                + SEditorGUILayout.Vertical("helpbox")
                .Content(
                    SEditorGUILayout.Action(() =>
                    {
                        if (value)
                        {
                            content?.Render();
                        }
                    })
                )
            )
            .Render();
        }
    }
}