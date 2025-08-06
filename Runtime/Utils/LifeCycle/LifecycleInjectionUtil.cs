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

                ConstructorInfo ctor = null;
                foreach (var constructor in fieldType.GetConstructors())
                {
                    var parameters = constructor.GetParameters();
                    if (parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(target.GetType()))
                    {
                        ctor = constructor;
                        break;
                    }
                }

                if (ctor == null)
                {
                    Debug.LogError($"[LifecycleInject] {fieldType.Name}에 {target.GetType().Name}을 인자로 받는 적합한 생성자가 없음");
                    continue;
                }

                var instance = ctor.Invoke(new[] { target });
                field.SetValue(target, instance);
            }
        }

        public static void OnEnable(object target) => CallLifecycleMethod(target, enable: true);
        public static void Start(object target) => CallStartMethod(target);
        public static void OnDisable(object target) => CallLifecycleMethod(target, enable: false);

        private static void CallLifecycleMethod(object target, bool enable)
        {
            var fields = target.GetType().GetFields(_flags);

            foreach (var field in fields)
            {
                if (field.GetCustomAttribute<LifecycleInjectAttribute>() == null)
                    continue;

                if (field.GetValue(target) is ILifecycleInjectable injectable)
                {
                    if (enable)
                        injectable.OnEnable();
                    else
                        injectable.OnDisable();
                }
            }
        }

        private static void CallStartMethod(object target)
        {
            var fields = target.GetType().GetFields(_flags);

            foreach (var field in fields)
            {
                if (field.GetCustomAttribute<LifecycleInjectAttribute>() == null)
                    continue;

                if (field.GetValue(target) is ILifecycleInjectable injectable)
                {
                    injectable.Start();
                }
            }
        }
    }
}