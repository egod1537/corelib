using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Corelib.SUI;
using UnityEngine.Events;

public class InspectorFactory<TBase> where TBase : class
{
    private readonly Dictionary<Type, MethodInfo> _renderGroupMethods;
    private readonly Dictionary<string, bool> _foldoutStates = new Dictionary<string, bool>();

    public InspectorFactory()
    {
        _renderGroupMethods = new Dictionary<Type, MethodInfo>();
        var inspectorTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(asm => asm.GetTypes())
            .Where(t => t.IsClass && t.IsSealed && t.IsAbstract && t.Name.EndsWith("Inspector"));

        foreach (var type in inspectorTypes)
        {
            var renderGroupMethod = type.GetMethod("RenderGroup", BindingFlags.Public | BindingFlags.Static);
            if (renderGroupMethod == null) continue;

            var parameters = renderGroupMethod.GetParameters();
            if (parameters.Length == 3 && typeof(TBase).IsAssignableFrom(parameters[0].ParameterType))
            {
                _renderGroupMethods[parameters[0].ParameterType] = renderGroupMethod;
            }
        }
    }

    public SUIElement Render(TBase model)
    {
        if (model == null) return SUIElement.Empty();

        var hierarchy = new List<Type>();
        var currentType = model.GetType();

        while (currentType != null && typeof(TBase).IsAssignableFrom(currentType))
        {
            hierarchy.Add(currentType);
            currentType = currentType.BaseType;
        }

        hierarchy.Reverse();

        var tree = new SUIElement();
        foreach (var modelType in hierarchy)
        {
            if (_renderGroupMethods.TryGetValue(modelType, out var renderGroupMethod))
            {
                string foldoutKey = $"{model.GetHashCode()}_{modelType.Name}";
                if (!_foldoutStates.ContainsKey(foldoutKey))
                {
                    _foldoutStates[foldoutKey] = true;
                }

                var parameters = new object[]
                {
                    model,
                    _foldoutStates[foldoutKey],
                    new UnityAction<bool>(v => _foldoutStates[foldoutKey] = v)
                };

                tree += (SUIElement)renderGroupMethod.Invoke(null, parameters);
            }
        }
        return tree;
    }
}