using UnityEngine;

namespace Corelib.Utils
{
    public static class ExFloat
    {
        public static float SafeRatio(this float value, float divisor)
        {
            if (Mathf.Approximately(0f, divisor)) return 0f;
            return Mathf.Max(0f, value / divisor);
        }
    }
}