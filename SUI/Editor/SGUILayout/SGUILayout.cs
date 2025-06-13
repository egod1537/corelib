using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SGUILayout : SUIElement
    {
        protected SGUILayout(UnityAction onRender) : base(onRender)
        {
        }

        public static SUIElement Label(string text) =>
            new SGUILayout(() => GUILayout.Label(text));

        public static SUIElement Space(float space) =>
            new SGUILayout(() => GUILayout.Space(space));

        public static SGUILayoutHorizontal Horizontal(SUIElement content = null) =>
            new SGUILayoutHorizontal(content);

        public static SGUILayoutVertical Vertical(SUIElement content = null) =>
            new SGUILayoutVertical(content);

        public static SUIElement FlexibleSpace() =>
            new SGUILayout(() => GUILayout.FlexibleSpace());

        public static SGUILayoutButton Button(string label)
            => new SGUILayoutButton(label);

        public static SGUILayoutSelectionGrid SelectionGrid(int value, List<GUIContent> guiContents, int columnCount = 1)
            => new SGUILayoutSelectionGrid(value, guiContents, columnCount);
    }
}