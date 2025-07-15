using UnityEngine;

namespace Corelib.Utils
{
    public class DefaultChunkMeshCreator : IChunkMeshCreator
    {
        public virtual GameObject Create(string name = "")
        {
            GameObject go = new GameObject();
            go.name = name;
            go.layer = LayerMask.NameToLayer("Landscape");

            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            go.AddComponent<MeshCollider>();

            return go;
        }
    }
}