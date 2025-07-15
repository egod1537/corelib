using UnityEngine;

namespace Corelib.Utils
{
    public interface IChunkMeshCreator
    {
        public GameObject Create(string name = "");
    }
}