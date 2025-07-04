using UnityEngine;

namespace Corelib.Utils
{
    public class PBox
    {
        public Vector3 center;
        public Vector3 size;

        public Vector3 Min => center - size * 0.5f;
        public Vector3 Max => center + size * 0.5f;

        public PBox(Vector3 center, Vector3 size)
        {
            this.center = center;
            this.size = size;
        }

        public static PBox FromMinMax(Vector3 min, Vector3 max)
        {
            var size = max - min;
            var center = min + size * 0.5f;
            return new PBox(center, size);
        }

        public static PBox FromTriangle(PTriangle tri)
        {
            var min = Vector3.Min(tri.v0, Vector3.Min(tri.v1, tri.v2));
            var max = Vector3.Max(tri.v0, Vector3.Max(tri.v1, tri.v2));
            return FromMinMax(min, max);
        }

        public bool Contains(Vector3 point)
        {
            Vector3 min = Min;
            Vector3 max = Max;
            return (point.x >= min.x && point.x <= max.x) &&
                   (point.y >= min.y && point.y <= max.y) &&
                   (point.z >= min.z && point.z <= max.z);
        }

        public bool Intersects(PBox other)
        {
            Vector3 aMin = Min;
            Vector3 aMax = Max;
            Vector3 bMin = other.Min;
            Vector3 bMax = other.Max;

            return (aMin.x <= bMax.x && aMax.x >= bMin.x) &&
                   (aMin.y <= bMax.y && aMax.y >= bMin.y) &&
                   (aMin.z <= bMax.z && aMax.z >= bMin.z);
        }

        public bool Intersects(Ray ray, out float distance)
        {
            return ToBounds().IntersectRay(ray, out distance);
        }

        public float Volume => size.x * size.y * size.z;

        public Bounds ToBounds() => new Bounds(center, size);

        public static bool DoBoxesIntersect(PBox a, PBox b)
        {
            return (a.Min.x <= b.Max.x && a.Max.x >= b.Min.x) &&
                   (a.Min.y <= b.Max.y && a.Max.y >= b.Min.y) &&
                   (a.Min.z <= b.Max.z && a.Max.z >= b.Min.z);
        }
    }

    public static class PBoxExtensions
    {
        public static PBox FromTriangle(this PTriangle tri)
        {
            var min = Vector3.Min(tri.v0, Vector3.Min(tri.v1, tri.v2));
            var max = Vector3.Max(tri.v0, Vector3.Max(tri.v1, tri.v2));
            return PBox.FromMinMax(min, max);
        }
    }
}