using System.Reflection;
using UnityEngine;

namespace Corelib.Utils
{
    public static class LifecycleInjectionUtil
    {
        private static readonly BindingFlags _flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        public static void ConstructLifecycleObjects(object target)
        {
            var fields = target.GetType().GetFields(_flags);

            foreach (var field in fields)
            {
                if (field.GetCustomAttribute<LifecycleInjectAttribute>() == null)
                    continue;

                if (field.GetValue(target) != null)
                    continue;

                var fieldType = field.FieldType;
                var ctor = fieldType.GetConstructor(new[] { target.GetType() });
                if (ctor == null)
                {
                    Debug.LogError($"[LifecycleInject] {fieldType.Name}에 {target.GetType().Name} 생성자가 없음");
                    continue;
                }

                var instance = ctor.Invoke(new[] { target });
                field.SetValue(target, instance);
            }
        }

        public static void OnEnable(object target) => CallLifecycleMethod(target, enable: true);
        public static void OnDisable(object target) => CallLifecycleMethod(target, enable: false);

        private static void CallLifecycleMethod(object target, bool enable)
        {
            var fields = target.GetType().GetFields(_flags);

            foreach (var field in fields)
            {
                if (field.GetCustomAttribute<LifecycleInjectAttribute>() == null)
                    continue;

                var value = field.GetValue(target);
                if (value is not ILifecycleInjectable injectable)
                    continue;

                if (enable)
                    injectable.OnEnable();
                else
                    injectable.OnDisable();
            }
        }
    }
}
