using System;

namespace Corelib.Utils
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class AutoModelSubscribeAttribute : Attribute
    {
        public string EventName { get; }

        public AutoModelSubscribeAttribute(string eventName)
        {
            EventName = eventName;
        }
    }
}
