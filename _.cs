using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
namespace Corelib.Utils
{
    public static class _
    {
        public static bool IsSerializable(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            Type type = obj.GetType();

            return type.IsSerializable || typeof(ISerializable).IsAssignableFrom(type);
        }

        public static void Swap<T>(ref T x, ref T y)
        {
            (x, y) = (y, x);
        }
    }
}