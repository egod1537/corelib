using System;
using System.Collections.Generic;
using System.Linq;
using Corelib.SUI;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UObject = UnityEngine.Object;

namespace Corelib.Utils
{
    public static class SerializableDictionaryInspector
    {
        public static SUIElement Render<TKey, TValue>(
            SerializableDictionary<TKey, TValue> dictionary,
            Action onModified = null)
        {
            if (dictionary == null) return SEditorGUILayout.Label($"Dictionary<{nameof(TKey)}, {nameof(TValue)}>: (null)");

            TKey keyToRemove = default;
            bool wantsToRemove = false;

            TKey newKey = default;
            TValue newValue = default;
            bool wantsToAdd = false;

            var container = SEditorGUILayout.Vertical();
            var elements = new List<SUIElement>();

            var originalLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 60f;

            foreach (var kvp in dictionary.ToList())
            {
                var currentKey = kvp.Key;
                elements.Add(
                    SEditorGUILayout.Horizontal()
                        .Content(
                            RenderField("Key", currentKey, null, true) +
                            RenderField("Value", kvp.Value, (val) =>
                            {
                                dictionary[currentKey] = val;
                                onModified?.Invoke();
                            }) +
                            SEditorGUILayout.Button("-").Width(20)
                                .OnClick(() => { keyToRemove = currentKey; wantsToRemove = true; })
                        )
                );
            }

            elements.Add(SEditorGUILayout.Space(5));
            elements.Add(
                SEditorGUILayout.Horizontal()
                    .Content(
                        RenderField("New Key", newKey, val => newKey = val) +
                        RenderField("New Value", newValue, val => newValue = val) +
                        SEditorGUILayout.Button("+").Width(20)
                            .OnClick(() =>
                            {
                                if (newKey != null && !dictionary.ContainsKey(newKey)) wantsToAdd = true;
                                else Debug.LogWarning($"Key '{newKey}' is null or already exists.");
                            })
                    )
            );

            elements.Add(SEditorGUILayout.Action(() => EditorGUIUtility.labelWidth = originalLabelWidth));

            if (elements.Any())
            {
                container.Content(elements.Aggregate((curr, next) => curr + next));
            }

            if (wantsToRemove)
            {
                dictionary.Remove(keyToRemove);
                onModified?.Invoke();
            }

            if (wantsToAdd)
            {
                dictionary.Add(newKey, newValue);
                onModified?.Invoke();
            }

            return SEditorGUILayout.Group($"Dictionary<{typeof(TKey).Name}, {typeof(TValue).Name}>")
                .Content(container);
        }

        private static SUIElement RenderField<T>(string label, T value, Action<T> onValueChanged, bool readOnly = false)
        {
            using (new EditorGUI.DisabledScope(readOnly))
            {
                var type = typeof(T);

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
                if (type.IsEnum)
                {
                    return SEditorGUILayout.Enum(label, (Enum)(object)value)
                        .OnValueChanged(v => onValueChanged?.Invoke((T)(object)v));
                }
                if (type == typeof(GameObject))
                {
                    return SEditorGUILayout.Object(label, value as GameObject, type)
                        .OnValueChanged(v => onValueChanged?.Invoke((T)(object)v));
                }
                if (typeof(UObject).IsAssignableFrom(type))
                {
                    return SEditorGUILayout.Object(label, value as UObject, type)
                        .OnValueChanged(v => onValueChanged?.Invoke((T)(object)v));
                }
                if (type == typeof(Vector3))
                {
                    return SEditorGUILayout.Vector3(label, (Vector3)(object)value)
                        .OnValueChanged(v => onValueChanged?.Invoke((T)(object)v));
                }
                if (type == typeof(Vector3Int))
                {
                    return SEditorGUILayout.Vector3Int(label, (Vector3Int)(object)value)
                        .OnValueChanged(v => onValueChanged?.Invoke((T)(object)v));
                }

                return SEditorGUILayout.Label($"{label}: Type not supported");
            }
        }
    }
}