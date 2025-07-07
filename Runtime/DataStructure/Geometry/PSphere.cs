using UnityEngine;

namespace Corelib.Utils
{
    public class PSphere
    {
        public Vector3 center;
        public float radius;

        public PSphere(Vector3 center, float radius)
        {
            this.center = center;
            this.radius = Mathf.Max(0f, radius);
        }

        public bool Contains(Vector3 point)
        {
            return Vector3.SqrMagnitude(point - center) <= radius * radius;
        }

        public bool Intersects(PSphere other)
        {
            float r = radius + other.radius;
            return Vector3.SqrMagnitude(center - other.center) <= r * r;
        }

        public bool Intersects(Ray ray, out float distance)
        {
            Vector3 oc = ray.origin - center;
            float b = Vector3.Dot(oc, ray.direction);
            float c = oc.sqrMagnitude - radius * radius;
            float discriminant = b * b - c;
            if (discriminant > 0f)
            {
                float sqrtD = Mathf.Sqrt(discriminant);
                float t = -b - sqrtD;
                if (t >= 0f)
                {
                    distance = t;
                    return true;
                }
                t = -b + sqrtD;
                if (t >= 0f)
                {
                    distance = t;
                    return true;
                }
            }
            distance = 0f;
            return false;
        }

        public float Volume => 4f / 3f * Mathf.PI * radius * radius * radius;

        public Vector3Int Min => Vector3Int.FloorToInt(center - Vector3.one * radius);
        public Vector3Int Max => Vector3Int.CeilToInt(center + Vector3.one * radius);
    }
}
