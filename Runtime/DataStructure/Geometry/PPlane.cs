using UnityEngine;

namespace Corelib.Utils
{
    public class PPlane
    {
        public Vector3Int min, max;
        public Vector3Int normal;

        public PPlane(Vector3Int min, Vector3Int max, Vector3Int normal)
        {
            this.min = min;
            this.max = max;
            this.normal = normal;
        }
    }
}