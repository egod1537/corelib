using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Corelib.Utils
{
    public class ScriptableModelConfig<T> : SerializedScriptableObject where T : ICloneable<T>
    {
        [SerializeField]
        public T template;
        public T Get() => template.Clone();
    }
}