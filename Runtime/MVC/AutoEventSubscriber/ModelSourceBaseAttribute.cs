using System;

namespace Corelib.Utils
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class ModelSourceBaseAttribute : Attribute { }
}
