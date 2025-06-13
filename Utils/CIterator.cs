using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corelib.Utils
{
    public static class CIterator
    {
        public static IEnumerable<Vector2Int> GetArray2D(int x, int y)
        {
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    yield return new Vector2Int(i, j);
        }
        public static IEnumerable<Vector2Int> GetArray2D(Vector2Int size)
        {
            for (int i = 0; i < size.x; i++)
                for (int j = 0; j < size.y; j++)
                    yield return new Vector2Int(i, j);
        }
        public static IEnumerable<Vector3Int> GetArray3D(int x, int y, int z)
        {
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    for (int k = 0; k < z; k++)
                        yield return new Vector3Int(i, j, k);
        }
        public static IEnumerable<Vector3Int> GetArray3D(Vector3Int size)
        {
            for (int i = 0; i < size.x; i++)
                for (int j = 0; j < size.y; j++)
                    for (int k = 0; k < size.z; k++)
                        yield return new Vector3Int(i, j, k);
        }
    }
}
