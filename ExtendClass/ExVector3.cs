using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corelib.Utils
{
    public static class ExVector3
    {
        public static void Deconstruct(this Vector3 vec, out float x, out float y, out float z)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }

        public static Vector3Int FloorToInt(this Vector3 vec)
            => new Vector3Int(
                Mathf.FloorToInt(vec.x),
                Mathf.FloorToInt(vec.y),
                Mathf.FloorToInt(vec.z)
            );

        public static Vector3Int CeilToInt(this Vector3 vec)
            => new Vector3Int(
                Mathf.CeilToInt(vec.x),
                Mathf.CeilToInt(vec.y),
                Mathf.CeilToInt(vec.z)
            );

        public static List<float> Flatten(this Vector3 vec)
            => new List<float>() { vec.x, vec.y, vec.z };

        public static float GetAt(this Vector3 vec, int idx)
        {
            switch (idx)
            {
                case 0: return vec.x;
                case 1: return vec.y;
                case 2: return vec.z;
            }
            return -1;
        }

        public static Vector3 SetAt(this ref Vector3 vec, int idx, float value)
        {
            switch (idx)
            {
                case 0: { vec.x = value; } break;
                case 1: { vec.y = value; } break;
                case 2: { vec.z = value; } break;
            }
            return vec;
        }

        public static Vector3Int RoundToInt(this Vector3 vec)
            => new Vector3Int(
                Mathf.RoundToInt(vec.x),
                Mathf.RoundToInt(vec.y),
                Mathf.RoundToInt(vec.z)
            );

        public static Vector3 Centroid(this Vector3 vec, List<Vector3> vecs)
        {
            foreach (Vector3 v in vecs) vec += v;
            int len = vecs.Count;
            vec /= len;
            return vec;
        }
    }
}

