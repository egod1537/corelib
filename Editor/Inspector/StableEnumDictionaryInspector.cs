using System;
using System.Collections.Generic;
using System.Linq;
using Corelib.SUI;
using UnityEditor;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace Corelib.Utils
{
    public static class StableEnumDictionaryInspector
    {
        private static class TempCache<TKey, TValue>
        {
            public static TKey NewKey;
            public static TValue NewValue;
        }

        public static SUIElement Render<TKey, TValue>(
            StableEnumDictionary<TKey, TValue> dictionary,
            Action onModified = null)
            where TKey : Enum
        {
            if (dictionary == null)
                return SEditorGUILayout.Label(
                    $"StableEnumDictionary<{typeof(TKey).Name}, {typeof(TValue).Name}>: (null)");

            // 캐시 불러오기
            var newKey = TempCache<TKey, TValue>.NewKey;
            var newValue = TempCache<TKey, TValue>.NewValue;

            var container = SEditorGUILayout.Vertical();
            var elements = new List<SUIElement>();

            var originalLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 60f;

            // 기존 항목 렌더링
            foreach (var kvp in dictionary.EnumPairs.ToList())
            {
                var currentKey = kvp.Key;
                elements.Add(
                    SEditorGUILayout.Horizontal()
                        .Content(
                            RenderField("Key", currentKey, _ => { }, readOnly: true)
                            + RenderField("Value", kvp.Value, v =>
                              {
                                  dictionary[currentKey] = v;
                                  onModified?.Invoke();
                                  GUI.changed = true;
                              })
                            + SEditorGUILayout.Button("-").Width(20)
                                .OnClick(() =>
                                {
                                    dictionary.Remove(currentKey);
                                    onModified?.Invoke();
                                    GUI.changed = true;
                                })
                        )
                );
            }

            elements.Add(SEditorGUILayout.Separator());
            elements.Add(SEditorGUILayout.Space(2));

            elements.Add(
                SEditorGUILayout.Horizontal()
                    .Content(
                        RenderField("New Key", newKey, val =>
                        {
                            newKey = val;
                            TempCache<TKey, TValue>.NewKey = val;
                        })
                        + RenderField("New Value", newValue, val =>
                        {
                            newValue = val;
                            TempCache<TKey, TValue>.NewValue = val;   // ← 핵심!
                        })
                        + SEditorGUILayout.Button("+").Width(20)
                            .OnClick(() =>
                            {
                                if (dictionary.ContainsKey(newKey))
                                {
                                    Debug.LogError($"Key '{newKey}' 는 이미 존재합니다.");
                                    return;
                                }

                                dictionary.Add(newKey, newValue);
                                newKey = default;
                                newValue = default;
                                onModified?.Invoke();
                                GUI.changed = true;
                            })
                    )
            );

            // labelWidth 원복
            elements.Add(SEditorGUILayout.Action(() =>
                EditorGUIUtility.labelWidth = originalLabelWidth));

            if (elements.Any())
                container.Content(elements.Aggregate((curr, next) => curr + next));

            // 캐시 저장
            TempCache<TKey, TValue>.NewKey = newKey;
            TempCache<TKey, TValue>.NewValue = newValue;

            return SEditorGUILayout.Group($"Dictionary<{typeof(TKey).Name}, {typeof(TValue).Name}>")
                                    .Content(container);
        }

        private static SUIElement RenderField<T>(
            string label, T value, Action<T> onValueChanged,
            bool readOnly = false)
        {
            using (new EditorGUI.DisabledScope(readOnly))
            {
                var type = typeof(T);

                if (typeof(UObject).IsAssignableFrom(type))
                {
                    return SEditorGUILayout.Object(label, value as UObject, type)
                        .OnValueChanged(v => onValueChanged?.Invoke((T)(object)v));
                }
                if (type.IsEnum)
                {
                    var enumValue = value == null
                        ? (Enum)Enum.GetValues(type).GetValue(0)   // 🔑 NRE 방지
                        : (Enum)(object)value;

                    return SEditorGUILayout.Enum(label, enumValue)
                        .OnValueChanged(v => onValueChanged?.Invoke((T)(object)v));
                }
                if (type == typeof(int))
                {
                    return SEditorGUILayout.Int(label, Convert.ToInt32(value))
                        .OnValueChanged(v => onValueChanged?.Invoke((T)(object)v));
                }
                if (type == typeof(float))
                {
                    return SEditorGUILayout.Float(label, Convert.ToSingle(value))
                        .OnValueChanged(v => onValueChanged?.Invoke((T)(object)v));
                }
                if (type == typeof(string))
                {
                    return SEditorGUILayout.Text(label, value as string)
                        .OnValueChanged(v => onValueChanged?.Invoke((T)(object)v));
                }
                if (type == typeof(Vector3))
                {
                    return SEditorGUILayout.Vector3(label,
                                value == null ? default : (Vector3)(object)value)
                        .OnValueChanged(v => onValueChanged?.Invoke((T)(object)v));
                }
                if (type == typeof(Vector3Int))
                {
                    return SEditorGUILayout.Vector3Int(label,
                                value == null ? default : (Vector3Int)(object)value)
                        .OnValueChanged(v => onValueChanged?.Invoke((T)(object)v));
                }

                return SEditorGUILayout.Label($"{label}: Unsupported type '{type.Name}'");
            }
        }
    }
}
