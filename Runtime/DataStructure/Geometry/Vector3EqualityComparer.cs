using System.Collections.Generic;
using UnityEngine;

namespace Corelib.Utils
{
    public class Vector3EqualityComparer : IEqualityComparer<Vector3>
    {
        private readonly float precision;

        public Vector3EqualityComparer(float precision)
        {
            this.precision = 1f / precision;
        }

        public bool Equals(Vector3 v1, Vector3 v2)
        {
            return (int)(v1.x * precision) == (int)(v2.x * precision) &&
                   (int)(v1.y * precision) == (int)(v2.y * precision) &&
                   (int)(v1.z * precision) == (int)(v2.z * precision);
        }

        public int GetHashCode(Vector3 obj)
        {
            int hashX = (int)(obj.x * precision);
            int hashY = (int)(obj.y * precision);
            int hashZ = (int)(obj.z * precision);
            return (hashX, hashY, hashZ).GetHashCode();
        }
    }
}