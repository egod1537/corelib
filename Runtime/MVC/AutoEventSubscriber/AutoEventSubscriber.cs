using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using System.Collections.Generic;
using System;
using Corelib.Utils;
using System.Linq;

namespace Corelib.Utils
{
    public class AutoEventSubscriber : ILifecycleInjectable
    {
        private readonly MonoBehaviour mono;
        private readonly Dictionary<Delegate, (UnityEventBase, MethodInfo)> _subscribedActions = new();

        public AutoEventSubscriber(MonoBehaviour mono)
        {
            this.mono = mono;
        }

        public void OnEnable()
        {
            var targetType = mono.GetType();
            var context = mono as UnityEngine.Object;

            foreach (var method in targetType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                var attributes = method.GetCustomAttributes<AutoSubscribeAttribute>(true);
                foreach (var attribute in attributes)
                {
                    var eventField = targetType.GetField(attribute.EventName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    if (eventField == null)
                    {
                        Debug.LogError($"[AutoSubscriber] Event field '{attribute.EventName}' not found in {targetType.Name}.", context);
                        continue;
                    }

                    if (eventField.GetValue(mono) is not UnityEventBase unityEvent)
                    {
                        Debug.LogError($"[AutoSubscriber] Field '{attribute.EventName}' is not a UnityEventBase.", context);
                        continue;
                    }

                    try
                    {
                        var action = CreateAction(method, unityEvent);
                        if (action != null)
                        {
                            unityEvent.GetType().GetMethod("AddListener")?.Invoke(unityEvent, new object[] { action });
                            _subscribedActions[action] = (unityEvent, method);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[AutoSubscriber] Failed to subscribe '{method.Name}' to '{attribute.EventName}'. Error: {e.Message}", context);
                    }
                }
            }
        }

        public void OnDisable()
        {
            foreach (var (action, (unityEvent, _)) in _subscribedActions)
            {
                if (unityEvent != null)
                {
                    unityEvent.GetType().GetMethod("RemoveListener")?.Invoke(unityEvent, new object[] { action });
                }
            }
            _subscribedActions.Clear();
        }

        private Delegate CreateAction(MethodInfo method, UnityEventBase unityEvent)
        {
            var delegateType = unityEvent.GetType().GetMethod("AddListener").GetParameters()[0].ParameterType;
            return Delegate.CreateDelegate(delegateType, mono, method);
        }
    }
}
