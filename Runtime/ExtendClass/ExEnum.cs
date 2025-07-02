using System;

namespace Corelib.Utils
{
    public static class ExEnum
    {
        public static int Count<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum)).Length;
        }
    }
}