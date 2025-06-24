using UnityEngine;
using UnityEngine.Events;

namespace Tools.BlockGrid
{
    public static class GizmosDrawer
    {
        public class ContainerBuilder
        {
            private UnityEvent onOpen = new();
            private UnityEvent onClose = new();

            public ContainerBuilder SetMatrix(Matrix4x4 mat)
            {
                onOpen.AddListener(() => { Gizmos.matrix = mat; });
                onClose.AddListener(() => { Gizmos.matrix = Matrix4x4.identity; });
                return this;
            }

            public ContainerBuilder SetColor(Color color)
            {
                onOpen.AddListener(() => { Gizmos.color = color; });
                onClose.AddListener(() => { Gizmos.color = Color.white; });
                return this;
            }

            public void Draw(UnityAction action)
            {
                onOpen.Invoke();
                action();
                onClose.Invoke();
            }
        }

        public static ContainerBuilder InContainer()
            => new ContainerBuilder();
    }
}