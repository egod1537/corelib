using System;

namespace Corelib.Utils
{
    public static class ExType
    {
        public static int GetEnumLength(this Type enumType)
            => Enum.GetValues(enumType).Length;
    }
}