using System;

namespace Corelib.Utils
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class AutoSubscribeAttribute : Attribute
    {
        public string EventName { get; }

        public AutoSubscribeAttribute(string eventName)
        {
            EventName = eventName;
        }
    }
}