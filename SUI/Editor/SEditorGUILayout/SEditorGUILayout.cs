using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UObject = UnityEngine.Object;

namespace Corelib.SUI
{
    public class SEditorGUILayout : SUIElement
    {
        protected SEditorGUILayout(UnityAction onRender) : base(onRender)
        {
        }

        public static SEditorGUILayoutLabel Label(string label)
            => new SEditorGUILayoutLabel(label);

        public static SEditorGUILayoutButton Button(string label)
            => new SEditorGUILayoutButton(label);

        public static SEditorGUILayoutHorizontal Horizontal(string style = "") =>
            new SEditorGUILayoutHorizontal(style);

        public static SEditorGUILayoutVertical Vertical(string style = "") =>
            new SEditorGUILayoutVertical(style);

        public static SEditorGUILayoutToggle Toggle(string label, bool value)
            => new SEditorGUILayoutToggle(label, value);

        public static SEditorGUILayoutSlider Slider(float value, float minValue = 0f, float maxValue = 1.0f)
            => new SEditorGUILayoutSlider(value, minValue, maxValue);

        public static SEditorGUILayoutFoldoutHeaderGroup FoldoutHeaderGroup(string prefix, bool foldout) =>
            new SEditorGUILayoutFoldoutHeaderGroup(prefix, foldout);

        public static SEditorGUILayoutFoldout Foldout(string prefix, bool foldout) =>
            new SEditorGUILayoutFoldout(prefix, foldout);

        public static SUIElement Action(UnityAction action) =>
            new SEditorGUILayout(() => action?.Invoke());

        public static SEditorGUILayoutVector3 Vector3(string prefix, Vector3 value)
            => new SEditorGUILayoutVector3(prefix, value);

        public static SEditorGUILayoutVector3Int Vector3Int(string prefix, Vector3Int value)
            => new SEditorGUILayoutVector3Int(prefix, value);

        public static SEditorGUILayoutInt Int(string prefix, int value)
            => new SEditorGUILayoutInt(prefix, value);

        public static SEditorGUILayoutFloat Float(string prefix, float value)
            => new SEditorGUILayoutFloat(prefix, value);

        public static SEditorGUILayoutMinMaxSlider MinMaxSlider(float minValue, float maxValue)
            => new SEditorGUILayoutMinMaxSlider(minValue, maxValue);

        public static SEditorGUILayoutProperty Property(SerializedProperty property)
            => new SEditorGUILayoutProperty(property);

        public static SEditorGUILayoutObject Object(string label, UObject obj, Type type)
            => new SEditorGUILayoutObject(label, obj, type);

        public static SEditorGUILayout ProgressBar(string label, float value)
            => new SEditorGUILayout(() =>
            {
                Rect rect = EditorGUILayout.BeginVertical();
                EditorGUI.ProgressBar(rect, value, label);
                EditorGUILayout.EndVertical();
            });

        public static SEditorGUILayoutEnum Enum(string label, Enum value)
            => new SEditorGUILayoutEnum(label, value);

        public static SEditorGUILayoutGroup Group(string title)
            => new SEditorGUILayoutGroup(title);

        public static SEditorGUILayout Separator(float thickness = 1.0f)
            => new SEditorGUILayout(() =>
            {
                GUILayout.Space(1f);
                Rect rect = EditorGUILayout.GetControlRect(false, thickness);
                rect.height = thickness;
                EditorGUI.DrawRect(rect, new Color(0.3f, 0.3f, 0.3f));
                GUILayout.Space(1f);
            });

        public static SEditorGUILayoutFoldGroup FoldGroup(string label, bool foldout)
            => new SEditorGUILayoutFoldGroup(label, foldout);

        public static SEditorGUILayout Space(float space)
            => new SEditorGUILayout(() => EditorGUILayout.Space(space));
    }
}