using UnityEngine;

namespace Corelib.Utils
{
    public class PCapsule
    {
        public Vector3 point1, point2;
        public float radius;
        public float height;
        public int direction;

        public PCapsule(Vector3 center, float radius, float height, int direction)
        {
            this.radius = Mathf.Max(0, radius);
            this.height = Mathf.Max(this.radius * 2, height);
            this.direction = direction;

            Vector3 axis;
            if (direction == 0) axis = Vector3.right;
            else if (direction == 2) axis = Vector3.forward;
            else axis = Vector3.up;

            Vector3 halfCylinder = axis * (this.height / 2f - this.radius);
            this.point1 = center + halfCylinder;
            this.point2 = center - halfCylinder;
        }

        public void ApplyTo(CapsuleCollider collider)
        {
            if (collider == null)
            {
                Debug.LogError("Target CapsuleCollider is null.");
                return;
            }

            collider.radius = this.radius;
            collider.height = this.height;
            collider.direction = this.direction;

            Vector3 worldCenter = (this.point1 + this.point2) / 2f;
            collider.center = collider.transform.InverseTransformPoint(worldCenter);
        }

        public bool Contains(Vector3 point)
        {
            Vector3 closestPointOnLine = ClosestPointOnLineSegment(point1, point2, point);
            float sqrDistance = Vector3.SqrMagnitude(point - closestPointOnLine);
            return sqrDistance <= radius * radius;
        }

        private Vector3 ClosestPointOnLineSegment(Vector3 start, Vector3 end, Vector3 point)
        {
            Vector3 segment = end - start;
            float t = Vector3.Dot(point - start, segment) / segment.sqrMagnitude;
            t = Mathf.Clamp01(t);
            return start + t * segment;
        }
    }
}