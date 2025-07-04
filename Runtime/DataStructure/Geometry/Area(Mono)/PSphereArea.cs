using UnityEngine;

namespace Corelib.Utils
{
    [ExecuteAlways]
    public class PSphereArea : MonoBehaviour
    {
        public float radius = 0.5f;
        public Color gizmoColor = new(0f, 1f, 0f, 0.5f);

        public PSphere Sphere => new PSphere(transform.position, radius);

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
