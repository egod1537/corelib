#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

namespace Corelib.Utils
{
    [CustomPropertyDrawer(typeof(ReferenceBindAttribute))]
    public class ReferenceBindDrawer : PropertyDrawer
    {
        private const float BtnWidth = 45f;
        private const string Prefix = "<ref>";

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            // ── ① 레이아웃 계산 (버튼은 항상 존재) ────────────────
            Rect fieldRect = new Rect(pos.x, pos.y, pos.width - BtnWidth - 2f, pos.height);
            Rect btnRect = new Rect(fieldRect.xMax + 2f, pos.y, BtnWidth, pos.height);

            // ── ② ObjectField 직접 그리기 ───────────────────────
            EditorGUI.ObjectField(fieldRect, prop, fieldInfo.FieldType, label);

            // ── ③ Find 버튼 ─────────────────────────────────────
            using (new EditorGUI.DisabledScope(prop.objectReferenceValue != null))
            {
                if (!GUI.Button(btnRect, "Find")) return;
            }

            var mono = prop.serializedObject.targetObject as MonoBehaviour;
            if (mono == null) return;

            // 검색: "<ui> {prop.name}" → "{prop.name}"
            string prefixed = $"{Prefix} {prop.name}";
            var tr = FindDeep(mono.transform, prefixed) ??
                     FindDeep(mono.transform, prop.name);

            if (tr == null)
            {
                Debug.LogError($"[Bind] '{prefixed}' 또는 '{prop.name}' 못 찾음", mono);
                return;
            }

            // 타입 매칭
            var t = fieldInfo.FieldType;
            object valObj = t == typeof(GameObject) ? tr.gameObject
                          : typeof(Component).IsAssignableFrom(t) ? tr.GetComponent(t)
                          : null;

            if (valObj == null)
            {
                Debug.LogError($"[Bind] 타입 불일치: {t}", mono);
                return;
            }

            // Undo + 값 세팅
            Undo.RecordObject(mono, "Bind UI Element");
            fieldInfo.SetValue(mono, valObj);
            prop.objectReferenceValue = valObj as UnityEngine.Object;

            prop.serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(mono);
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label) =>
            EditorGUI.GetPropertyHeight(prop, label, true);

        // ─────────────────── Helper ───────────────────
        private static Transform FindDeep(Transform root, string name)
        {
            foreach (var t in root.GetComponentsInChildren<Transform>(true))
                if (t.name.Equals(name, StringComparison.Ordinal)) return t;
            return null;
        }
    }
}
#endif
