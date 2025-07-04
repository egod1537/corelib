using System;
using UnityEngine;

namespace Corelib.Utils
{
    [Serializable]
    public struct StableEnum<T> : ISerializationCallbackReceiver where T : struct, Enum
    {
        [SerializeField] private T value;

        [SerializeField, HideInInspector] private string name;

        public static implicit operator T(StableEnum<T> se) => se.value;
        public static implicit operator StableEnum<T>(T e) => new StableEnum<T> { value = e, name = e.ToString() };

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                name = value.ToString();
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            name = value.ToString();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(name) && Enum.TryParse(name, out T parsed))
                value = parsed;
        }
    }
}