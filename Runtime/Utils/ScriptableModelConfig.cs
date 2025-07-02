using UnityEngine;

namespace Corelib.Utils
{
    public class ScriptableModelConfig<T> : ScriptableObject where T : ICloneable<T>
    {
        [SerializeField]
        public T template;
        public T Get() => template.Clone();
    }
}