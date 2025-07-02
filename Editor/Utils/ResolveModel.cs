using UnityEditor;
using UnityEngine;

namespace Corelib.Utils
{
    public static class ResolveModel
    {
        public static T ResolveEditor<T>(T model, ScriptableModelConfig<T> modelConfig) where T : class, ICloneable<T>
        {
            if (!EditorApplication.isPlaying)
                return modelConfig?.template ?? model;
            else
                return model ?? modelConfig?.template;
        }
    }
}