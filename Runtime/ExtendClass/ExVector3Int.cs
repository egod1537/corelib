using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corelib.Utils
{
    public static class ExVector3Int
    {
        public static readonly List<Vector3Int> DIR6 = new()
    {
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 0, -1)
    };

        public static readonly List<Vector3Int> DIR26 = new();
        public static readonly List<Vector3Int> DIR27;

        static ExVector3Int()
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        if (x == 0 && y == 0 && z == 0)
                            continue;

                        DIR26.Add(new Vector3Int(x, y, z));
                    }
                }
            }

            DIR27 = new List<Vector3Int>(DIR26) { Vector3Int.zero };
        }

        public static void Deconstruct(this Vector3Int vec, out int x, out int y, out int z)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }

        public static int Area(this Vector3Int vec) => vec.x * vec.y * vec.z;

        public static IEnumerable<Vector3Int> Spread(Vector3Int from, Vector3Int to)
        {
            List<Vector3Int> spreads = new();
            for (int x = from.x; x <= to.x; x++)
                for (int y = from.y; y <= to.y; y++)
                    for (int z = from.z; z <= to.z; z++)
                        spreads.Add(new Vector3Int(x, y, z));
            return spreads;
        }

        public static List<int> Flatten(this Vector3Int vec)
            => new List<int>() { vec.x, vec.y, vec.z };

        public static Vector3 ToVector3(this Vector3Int vec)
            => new Vector3(vec.x, vec.y, vec.z);

        public static Vector3Int Min(this Vector3Int vec, Vector3Int other)
            => new Vector3Int(
                Math.Min(vec.x, other.x),
                Math.Min(vec.y, other.y),
                Math.Min(vec.z, other.z)
            );

        public static Vector3Int Max(this Vector3Int vec, Vector3Int other)
            => new Vector3Int(
                Math.Max(vec.x, other.x),
                Math.Max(vec.y, other.y),
                Math.Max(vec.z, other.z)
            );

        public static bool LessEqual(this Vector3Int vec, Vector3Int other)
            => vec.x <= other.x && vec.y <= other.y && vec.z <= other.z;
        public static bool Less(this Vector3Int vec, Vector3Int other)
            => vec.x < other.x && vec.y < other.y && vec.z < other.z;

        public static bool GreaterEqual(this Vector3Int vec, Vector3Int other)
            => vec.x >= other.x && vec.y >= other.y && vec.z >= other.z;
        public static bool Greater(this Vector3Int vec, Vector3Int other)
            => vec.x > other.x && vec.y > other.y && vec.z > other.z;

        public static bool InRange(this Vector3Int vec, Vector3Int l, Vector3Int r)
            => l.GreaterEqual(vec) && vec.LessEqual(r);

        public static List<int> ToArray(this Vector3Int vec)
            => new List<int>() { vec.x, vec.y, vec.z };

        public static Vector3Int RandomRange(this Vector3Int left, Vector3Int right, MT19937 rng = null)
        {
            if (rng == null)
                rng = MT19937.Create();
            return new Vector3Int(
                rng.NextInt(left.x, right.x),
                rng.NextInt(left.y, right.y),
                rng.NextInt(left.z, right.z)
            );
        }

        public static int GetAt(this Vector3Int vec, int idx)
        {
            switch (idx)
            {
                case 0: return vec.x;
                case 1: return vec.y;
                case 2: return vec.z;
            }
            return -1;
        }

        public static Vector3Int SetAt(this ref Vector3Int vec, int idx, int value)
        {
            switch (idx)
            {
                case 0: { vec.x = value; } break;
                case 1: { vec.y = value; } break;
                case 2: { vec.z = value; } break;
            }
            return vec;
        }

        public static Vector3Int Centroid(this Vector3Int vec, List<Vector3Int> vecs)
        {
            foreach (Vector3Int v in vecs) vec += v;
            int len = vecs.Count;
            return (vec.ToVector3() / len).RoundToInt();
        }
    }
}

