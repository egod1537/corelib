using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    // TODO
    public class SEditorGUI : SUIElement
    {
        protected SEditorGUI(UnityAction onRender) : base(onRender)
        {

        }
        protected SEditorGUI(Rect rect, UnityAction onRender) : base(rect, onRender)
        {

        }

        public static SEditorGUILayoutLabel Label(Rect rect, string label)
            => new SEditorGUILayoutLabel(label);

        public static SEditorGUILayoutToggle Toggle(Rect rect, string label, bool value)
            => new SEditorGUILayoutToggle(label, value);

        public static SEditorGUILayoutSlider Slider(Rect rect, float value, float minValue = 0f, float maxValue = 1.0f)
            => new SEditorGUILayoutSlider(value, minValue, maxValue);

        public static SEditorGUILayoutFoldoutHeaderGroup FoldoutHeaderGroup(string prefix, bool foldout) =>
            new SEditorGUILayoutFoldoutHeaderGroup(prefix, foldout);

        public static SUIElement Action(UnityAction action) =>
            new SEditorGUI(() => action?.Invoke());

        public static SEditorGUILayoutVector3Int Vector3Int(string prefix, Vector3Int value)
            => new SEditorGUILayoutVector3Int(prefix, value);

        public static SEditorGUILayoutInt Int(string prefix, int value)
            => new SEditorGUILayoutInt(prefix, value);

        public static SEditorGUILayoutMinMaxSlider MinMaxSlider(float minValue, float maxValue)
            => new SEditorGUILayoutMinMaxSlider(minValue, maxValue);
    }
}