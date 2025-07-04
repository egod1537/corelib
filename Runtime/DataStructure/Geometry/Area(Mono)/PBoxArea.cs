using UnityEngine;

namespace Corelib.Utils
{
    [ExecuteAlways]
    public class PBoxArea : MonoBehaviour
    {
        public Vector3 size = Vector3.one;
        public Color gizmoColor = new(0f, 1f, 0f, 0.5f);

        public PBox Box => new PBox(transform.position, size);

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(transform.position, size);
        }
    }
}
