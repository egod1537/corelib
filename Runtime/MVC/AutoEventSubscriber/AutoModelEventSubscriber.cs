using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Corelib.Utils
{
    public sealed class AutoModelEventSubscriber : ILifecycleInjectable
    {
        private readonly MonoBehaviour mono;
        private readonly Dictionary<Delegate, (UnityEventBase evt, MethodInfo method)> subscriptions = new();

        public AutoModelEventSubscriber(MonoBehaviour mono)
        {
            this.mono = mono;
        }

        public void OnEnable()
        {
            var targetType = mono.GetType();
            var ctx = mono as UnityEngine.Object;

            var taggedMethods = targetType.GetMethods(
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(m => m.GetCustomAttributes<AutoModelSubscribeAttribute>(true).Any())
                    .ToArray();

            if (taggedMethods.Length == 0)
                return;                         // 태그 없으면 그냥 종료 (경고 X)

            // 1. ModelSourceBase가 붙은 모든 필드 가져옴
            const BindingFlags FieldFlags =
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var modelFields = targetType
                .GetFields(FieldFlags)
                .Where(f => f.GetCustomAttribute<ModelSourceBaseAttribute>() != null)   // ★ 필터 필요
                .ToArray();

            if (modelFields.Length == 0)
            {
                Debug.LogError($"[AutoModelEventSubscriber] {targetType.Name}에 [ModelSourceBase] 필드가 없음.", ctx);
                return;
            }

            foreach (var method in taggedMethods)
            {
                var attrs = method.GetCustomAttributes<AutoModelSubscribeAttribute>(true);
                foreach (var attr in attrs)
                {
                    bool subscribed = false;

                    foreach (var modelField in modelFields)
                    {
                        var modelInstance = modelField.GetValue(mono);
                        if (modelInstance == null)
                        {
                            Debug.LogError($"[AutoModelSubscriber] '{modelField.Name}'가 null.", ctx);
                            continue;
                        }

                        var evtField = modelInstance.GetType()
                            .GetField(attr.EventName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                        if (evtField == null) continue; // 다음 모델 필드 탐색

                        if (evtField.GetValue(modelInstance) is not UnityEventBase unityEvent)
                        {
                            Debug.LogError($"[AutoModelSubscriber] '{attr.EventName}'는 UnityEventBase가 아님.", ctx);
                            continue;
                        }

                        try
                        {
                            var delegateType = unityEvent.GetType()
                                .GetMethod("AddListener")!.GetParameters()[0].ParameterType;
                            var action = Delegate.CreateDelegate(delegateType, mono, method);

                            unityEvent.GetType().GetMethod("AddListener")!.Invoke(unityEvent, new object[] { action });
                            subscriptions[action] = (unityEvent, method);
                            subscribed = true;
                            break; // 이벤트 찾았으면 다른 모델엔 안 탐색
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"[AutoModelSubscriber] '{method.Name}' 구독 실패 → {e.Message}", ctx);
                        }
                    }

                    if (!subscribed)
                        Debug.LogError($"[AutoModelSubscriber] '{attr.EventName}' 이벤트를 어떤 모델에서도 못 찾음.", ctx);
                }
            }
        }

        public void OnDisable()
        {
            foreach (var (del, (evt, _)) in subscriptions)
            {
                evt?.GetType().GetMethod("RemoveListener")?.Invoke(evt, new object[] { del });
            }
            subscriptions.Clear();
        }
    }
}
