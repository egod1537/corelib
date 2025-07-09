using UnityEngine;

namespace Corelib.Utils
{
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