using UnityEngine;

namespace Corelib.Utils
{
    [ExecuteAlways]
    public class PSphereArea : MonoBehaviour
    {
        public float radius = 0.5f;
        public Color gizmoColor = new(1f, 0.2f, 0f, 0.5f);

        public PSphere Sphere => new PSphere(transform.position, radius);

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Matrix4x4 prev = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawWireSphere(Vector3.zero, radius);
            Gizmos.matrix = prev;
        }
    }
}
