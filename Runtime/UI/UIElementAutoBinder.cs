using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.Utils
{
    public class AutoReferenceBinder : ILifecycleInjectable
    {
        private readonly MonoBehaviour _mono;
        private const string Prefix = "<ui>";

        public AutoReferenceBinder(MonoBehaviour mono)
        {
            _mono = mono;
            BindAllFields();
        }

        public void OnEnable() => BindAllFields();
        public void OnDisable() { }

        public void BindAllFields()
        {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var fields = _mono.GetType().GetFields(flags)
                              .Where(f => Attribute.IsDefined(f, typeof(ReferenceBindAttribute)));

            foreach (var field in fields)
            {
                if (field.GetValue(_mono) != null) continue;

                var tr = FindDeep(_mono.transform, $"{Prefix} {field.Name}") ??
                         FindDeep(_mono.transform, field.Name);

                if (tr == null)
                {
#if UNITY_EDITOR
                    Debug.LogWarning($"[AutoBind] '{field.Name}' 못 찾음", _mono);
#endif
                    continue;
                }

                object val = null;
                if (field.FieldType == typeof(GameObject))
                    val = tr.gameObject;
                else if (typeof(Component).IsAssignableFrom(field.FieldType))
                    val = tr.GetComponent(field.FieldType);

                if (val != null)
                    field.SetValue(_mono, val);
                else
#if UNITY_EDITOR
                    Debug.LogWarning($"[AutoBind] 타입 불일치: {field.FieldType}", _mono);
#endif
            }
        }

        private static Transform FindDeep(Transform root, string name) =>
            root.GetComponentsInChildren<Transform>(true)
                .FirstOrDefault(t => t.name.Equals(name, StringComparison.Ordinal));
    }
}
