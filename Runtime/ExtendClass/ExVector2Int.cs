using System;
using System.Collections.Generic;
using UnityEngine;

namespace Corelib.Utils
{
    public static class ExVector2Int
    {
        public static readonly List<Vector2Int> DIR4 = new()
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

        public static readonly List<Vector2Int> DIR8 = new()
    {
        new Vector2Int(1, 0), new Vector2Int(-1, 0),
        new Vector2Int(0, 1), new Vector2Int(0, -1),
        new Vector2Int(1, 1), new Vector2Int(1, -1),
        new Vector2Int(-1, 1), new Vector2Int(-1, -1)
    };

        public static readonly List<Vector2Int> DIR9 = new(DIR8) { Vector2Int.zero };

        public static void Deconstruct(this Vector2Int vec, out int x, out int y)
        {
            x = vec.x;
            y = vec.y;
        }

        public static int Area(this Vector2Int vec) => vec.x * vec.y;

        public static IEnumerable<Vector2Int> Spread(Vector2Int from, Vector2Int to)
        {
            List<Vector2Int> spreads = new();
            for (int x = from.x; x <= to.x; x++)
                for (int y = from.y; y <= to.y; y++)
                    spreads.Add(new Vector2Int(x, y));
            return spreads;
        }

        public static List<int> Flatten(this Vector2Int vec)
            => new List<int>() { vec.x, vec.y };

        public static Vector3 ToVector2(this Vector2Int vec)
            => new Vector2(vec.x, vec.y);

        public static Vector2Int Min(this Vector2Int vec, Vector2Int other)
            => new Vector2Int(
                Math.Min(vec.x, other.x),
                Math.Min(vec.y, other.y)
            );

        public static Vector2Int Max(this Vector2Int vec, Vector2Int other)
            => new Vector2Int(
                Math.Max(vec.x, other.x),
                Math.Max(vec.y, other.y)
            );

        public static bool LessEqual(this Vector2Int vec, Vector2Int other)
            => vec.x <= other.x && vec.y <= other.y;
        public static bool Less(this Vector2Int vec, Vector2Int other)
            => vec.x < other.x && vec.y < other.y;

        public static bool GreaterEqual(this Vector2Int vec, Vector2Int other)
            => vec.x >= other.x && vec.y >= other.y;
        public static bool Greater(this Vector2Int vec, Vector2Int other)
            => vec.x > other.x && vec.y > other.y;

        public static bool InRange(this Vector2Int vec, Vector2Int l, Vector2Int r)
            => l.GreaterEqual(vec) && vec.LessEqual(r);

        public static List<int> ToArray(this Vector2Int vec)
            => new List<int>() { vec.x, vec.y };

        public static Vector2Int RandomRange(this Vector2Int left, Vector2Int right, MT19937 rng = null)
        {
            if (rng == null)
                rng = MT19937.Create();
            return new Vector2Int(
                rng.NextInt(left.x, right.x),
                rng.NextInt(left.y, right.y)
            );
        }

        public static int GetAt(this Vector2Int vec, int idx)
        {
            switch (idx)
            {
                case 0: return vec.x;
                case 1: return vec.y;
            }
            return -1;
        }

        public static Vector2Int SetAt(this ref Vector2Int vec, int idx, int value)
        {
            switch (idx)
            {
                case 0: { vec.x = value; } break;
                case 1: { vec.y = value; } break;
            }
            return vec;
        }
    }
}