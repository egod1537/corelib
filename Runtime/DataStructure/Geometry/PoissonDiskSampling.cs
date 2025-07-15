using System.Collections.Generic;
using UnityEngine;

namespace Corelib.Utils
{
    public static class PoissonDiskSampling
    {
        private const int k = 30;

        public static List<PSphere> Generate(PBox box, float radius, int count, MT19937 rng = null)
        {
            if (box == null)
                return new List<PSphere>();
            return Generate(box.Min, box.Max, radius, count, rng);
        }

        public static List<PSphere> Generate(PBoxInt box, float radius, int count, MT19937 rng = null)
        {
            if (box == null)
                return new List<PSphere>();
            Vector3 min = box.topLeft;
            Vector3 max = box.bottomRight;
            return Generate(min, max, radius, count, rng);
        }

        public static List<PSphere> GenerateGrid(PBoxInt box, float radius, int count, MT19937 rng = null)
        {
            if (box == null)
                return new List<PSphere>();
            return GenerateGrid(box.topLeft, box.bottomRight, radius, count, rng);
        }

        private static List<PSphere> Generate(Vector3 min, Vector3 max, float radius, int count, MT19937 rng)
        {
            if (rng == null)
                rng = MT19937.Create();

            List<PSphere> result = new(count);
            if (radius <= 0f || count <= 0)
                return result;

            float cellSize = radius / Mathf.Sqrt(3f);
            var grid = new Dictionary<Vector3Int, Vector3>();
            List<Vector3> active = new();

            Vector3 first = new Vector3(
                rng.NextFloat(min.x, max.x),
                rng.NextFloat(min.y, max.y),
                rng.NextFloat(min.z, max.z)
            );
            grid[Cell(first, min, cellSize)] = first;
            active.Add(first);
            result.Add(new PSphere(first, radius));

            while (active.Count > 0 && result.Count < count)
            {
                int index = rng.NextInt(0, active.Count - 1);
                Vector3 point = active[index];
                bool found = false;

                for (int i = 0; i < k; i++)
                {
                    Vector3 dir = rng.NextVector3().normalized;
                    float dist = rng.NextFloat(radius, 2f * radius);
                    Vector3 candidate = point + dir * dist;
                    if (!InBounds(candidate, min, max))
                        continue;
                    if (IsFarEnough(candidate, grid, min, cellSize, radius))
                    {
                        grid[Cell(candidate, min, cellSize)] = candidate;
                        active.Add(candidate);
                        result.Add(new PSphere(candidate, radius));
                        found = true;
                        if (result.Count >= count)
                            break;
                    }
                }

                if (!found || result.Count >= count)
                    active.RemoveAt(index);
            }

            return result;
        }

        private static List<PSphere> GenerateGrid(Vector3Int min, Vector3Int max, float radius, int count, MT19937 rng)
        {
            if (rng == null)
                rng = MT19937.Create();

            List<PSphere> result = new(count);
            if (radius <= 0f || count <= 0)
                return result;

            float cellSize = radius / Mathf.Sqrt(3f);
            var grid = new Dictionary<Vector3Int, Vector3Int>();
            List<Vector3Int> active = new();

            Vector3Int first = new Vector3Int(
                rng.NextInt(min.x, max.x),
                rng.NextInt(min.y, max.y),
                rng.NextInt(min.z, max.z)
            );
            grid[Cell(first, min, cellSize)] = first;
            active.Add(first);
            result.Add(new PSphere(first, radius));

            while (active.Count > 0 && result.Count < count)
            {
                int index = rng.NextInt(0, active.Count - 1);
                Vector3Int point = active[index];
                bool found = false;

                for (int i = 0; i < k; i++)
                {
                    Vector3 dir = rng.NextVector3().normalized;
                    float dist = rng.NextFloat(radius, 2f * radius);
                    Vector3 candidateF = (Vector3)point + dir * dist;
                    Vector3Int candidate = Vector3Int.RoundToInt(candidateF);
                    if (!InBounds(candidate, min, max))
                        continue;
                    if (IsFarEnough(candidate, grid, min, cellSize, radius))
                    {
                        grid[Cell(candidate, min, cellSize)] = candidate;
                        active.Add(candidate);
                        result.Add(new PSphere(candidate, radius));
                        found = true;
                        if (result.Count >= count)
                            break;
                    }
                }

                if (!found || result.Count >= count)
                    active.RemoveAt(index);
            }

            return result;
        }

        private static Vector3Int Cell(Vector3 point, Vector3 origin, float cell)
        {
            Vector3 p = point - origin;
            return new Vector3Int(
                Mathf.FloorToInt(p.x / cell),
                Mathf.FloorToInt(p.y / cell),
                Mathf.FloorToInt(p.z / cell)
            );
        }

        private static bool InBounds(Vector3 p, Vector3 min, Vector3 max)
        {
            return p.x >= min.x && p.x <= max.x &&
                   p.y >= min.y && p.y <= max.y &&
                   p.z >= min.z && p.z <= max.z;
        }

        private static bool IsFarEnough(Vector3 candidate, Dictionary<Vector3Int, Vector3> grid, Vector3 origin, float cellSize, float radius)
        {
            Vector3Int c = Cell(candidate, origin, cellSize);
            for (int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    for (int z = -2; z <= 2; z++)
                    {
                        Vector3Int key = new Vector3Int(c.x + x, c.y + y, c.z + z);
                        if (grid.TryGetValue(key, out Vector3 sample))
                        {
                            if ((sample - candidate).sqrMagnitude < radius * radius)
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        private static Vector3Int Cell(Vector3Int point, Vector3Int origin, float cell)
        {
            Vector3 p = point - origin;
            return new Vector3Int(
                Mathf.FloorToInt(p.x / cell),
                Mathf.FloorToInt(p.y / cell),
                Mathf.FloorToInt(p.z / cell)
            );
        }

        private static bool InBounds(Vector3Int p, Vector3Int min, Vector3Int max)
        {
            return p.x >= min.x && p.x <= max.x &&
                   p.y >= min.y && p.y <= max.y &&
                   p.z >= min.z && p.z <= max.z;
        }

        private static bool IsFarEnough(Vector3Int candidate, Dictionary<Vector3Int, Vector3Int> grid, Vector3Int origin, float cellSize, float radius)
        {
            Vector3Int c = Cell(candidate, origin, cellSize);
            for (int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    for (int z = -2; z <= 2; z++)
                    {
                        Vector3Int key = new Vector3Int(c.x + x, c.y + y, c.z + z);
                        if (grid.TryGetValue(key, out Vector3Int sample))
                        {
                            if ((sample - candidate).sqrMagnitude < radius * radius)
                                return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
