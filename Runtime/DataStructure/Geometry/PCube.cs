using UnityEngine;

namespace Corelib.Utils
{
    public class PCube : PBox
    {
        public float Length
        {
            get => size.x;
            set => size = new Vector3(value, value, value);
        }

        public PCube(Vector3 center, float length)
            : base(center, new Vector3(length, length, length))
        {
        }

        public static PCube FromMin(Vector3 min, float length)
        {
            Vector3 center = min + Vector3.one * (length * 0.5f);
            return new PCube(center, length);
        }
    }
}