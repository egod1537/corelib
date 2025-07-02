using UnityEngine;

namespace Corelib.Utils
{
    public static class ExColor
    {
        public static Color Rainbow(this Color color, float weight)
        {
            float t = Mathf.InverseLerp(0, 1f, weight);
            float hue = Mathf.Lerp(0f, 0.83f, t);
            return color = Color.HSVToRGB(hue, 1f, 1f);
        }

        public static Color SetAlpha(this Color color, float value)
        {
            color.a = value;
            return color;
        }
    }
}