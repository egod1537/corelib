using UnityEngine;

namespace Corelib.Utils
{
    [ExecuteAlways]
    public class PCapsuleArea : MonoBehaviour
    {
        public Vector3 center = Vector3.zero;
        public float radius = 0.5f;
        public float height = 2f;
        [Tooltip("0 = X-Axis, 1 = Y-Axis, 2 = Z-Axis")]
        public int direction = 1;
        public Color gizmoColor = new(0f, 1f, 0f, 0.5f);

        public PCapsule Capsule => new PCapsule(transform.TransformPoint(center),
                                                radius,
                                                height,
                                                direction);
        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Matrix4x4 prev = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            DrawWireCapsule(center, Quaternion.identity,
                            radius,
                            height,
                            direction);
            Gizmos.matrix = prev;
        }

        private static void DrawWireCapsule(Vector3 center, Quaternion rotation, float radius, float height, int direction)
        {
            Vector3 axis;
            Vector3 up, right;

            if (direction == 0)
            {
                axis = Vector3.right;
                up = Vector3.up;
                right = Vector3.forward;
            }
            else if (direction == 2)
            {
                axis = Vector3.forward;
                up = Vector3.up;
                right = Vector3.right;
            }
            else
            {
                axis = Vector3.up;
                up = Vector3.forward;
                right = Vector3.right;
            }

            axis = rotation * axis;
            up = rotation * up;
            right = rotation * right;

            height = Mathf.Max(radius * 2, height);
            Vector3 halfCylinder = axis * (height / 2f - radius);
            Vector3 p1 = center + halfCylinder;
            Vector3 p2 = center - halfCylinder;

            Gizmos.DrawWireSphere(p1, radius);
            Gizmos.DrawWireSphere(p2, radius);

            Gizmos.DrawLine(p1 + right * radius, p2 + right * radius);
            Gizmos.DrawLine(p1 - right * radius, p2 - right * radius);
            Gizmos.DrawLine(p1 + up * radius, p2 + up * radius);
            Gizmos.DrawLine(p1 - up * radius, p2 - up * radius);
        }
    }
}