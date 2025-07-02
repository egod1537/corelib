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
        private static object _newKey;
        private static object _newValue;
        private static object _keyToRemove;

        private static bool _wantsToAdd;
        private static bool _wantsToRemove;

        private static int _targetInstanceID;

        public static SUIElement Render<TKey, TValue>(
            SerializableDictionary<TKey, TValue> dictionary,
            Action onModified = null)
        {
            if (dictionary == null) return SEditorGUILayout.Label($"Dictionary<{nameof(TKey)}, {nameof(TValue)}>: (null)");

            var dictionaryID = dictionary.GetHashCode();
            if (dictionaryID != _targetInstanceID)
            {
                _newKey = default(TKey);
                _newValue = default(TValue);
                _wantsToAdd = false;
                _wantsToRemove = false;
                _targetInstanceID = dictionaryID;
            }

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
                                .OnClick(() =>
                                {
                                    _keyToRemove = currentKey;
                                    _wantsToRemove = true;
                                    _targetInstanceID = dictionaryID;
                                })
                        )
                );
            }

            elements.Add(SEditorGUILayout.Separator());
            elements.Add(SEditorGUILayout.Space(2));

            elements.Add(
                SEditorGUILayout.Horizontal()
                    .Content(
                        RenderField("New Key", (TKey)_newKey, val => _newKey = val) +
                        RenderField("New Value", (TValue)_newValue, val => _newValue = val) +
                        SEditorGUILayout.Button("+").Width(20)
                            .OnClick(() =>
                            {
                                TKey key = (TKey)_newKey;
                                if (key != null && !dictionary.ContainsKey(key))
                                {
                                    _wantsToAdd = true;
                                    _targetInstanceID = dictionaryID;
                                }
                                else
                                {
                                    var keyString = key?.ToString() ?? "null";
                                    Debug.LogWarning($"Key '{keyString}' is null or already exists.");
                                }
                            })
                    )
            );

            elements.Add(SEditorGUILayout.Action(() => EditorGUIUtility.labelWidth = originalLabelWidth));

            if (elements.Any())
            {
                container.Content(elements.Aggregate((curr, next) => curr + next));
            }

            if (_wantsToRemove && _targetInstanceID == dictionaryID)
            {
                dictionary.Remove((TKey)_keyToRemove);
                _wantsToRemove = false;
                onModified?.Invoke();
            }

            if (_wantsToAdd && _targetInstanceID == dictionaryID)
            {
                dictionary.Add((TKey)_newKey, (TValue)_newValue);
                _newKey = default(TKey);
                _newValue = default(TValue);
                _wantsToAdd = false;
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
                if (typeof(ScriptableObject).IsAssignableFrom(type))
                {
                    return SEditorGUILayout.Object(label, value as ScriptableObject, type)
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