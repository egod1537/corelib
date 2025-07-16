using System.Collections.Generic;
using System.Linq;
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
            return GenerateByDartThrowing(box.Min, box.Max, radius, count, k, rng);
        }

        public static List<PSphere> Generate(PBoxInt box, float radius, int count, MT19937 rng = null)
        {
            if (box == null)
                return new List<PSphere>();
            return GenerateByDartThrowing(box.topLeft, box.bottomRight, radius, count, k, rng);
        }

        public static List<PSphere> GenerateFromPredefined(List<Vector3> predefinedPoints, float radius, int maxCount, MT19937 rng = null)
        {
            if (predefinedPoints == null || predefinedPoints.Count == 0)
                return new List<PSphere>();

            if (rng == null)
                rng = MT19937.Create();

            List<Vector3> shuffledPoints = new List<Vector3>(predefinedPoints);
            Shuffle(shuffledPoints, rng);

            List<PSphere> result = new();
            float sqrRadius = radius * radius;

            foreach (var candidate in shuffledPoints)
            {
                if (result.Count >= maxCount) break;
                if (IsFarEnoughFromAll(candidate, result, sqrRadius))
                {
                    result.Add(new PSphere(candidate, radius));
                }
            }

            return result;
        }

        private static List<PSphere> GenerateByDartThrowing(Vector3 min, Vector3 max, float radius, int count, int retries, MT19937 rng)
        {
            if (rng == null)
                rng = MT19937.Create();

            if (radius <= 0f || count <= 0)
                return new List<PSphere>();

            int candidateCount = count * retries;
            List<Vector3> candidates = new List<Vector3>(candidateCount);
            for (int i = 0; i < candidateCount; i++)
            {
                candidates.Add(new Vector3(
                    rng.NextFloat(min.x, max.x),
                    rng.NextFloat(min.y, max.y),
                    rng.NextFloat(min.z, max.z)
                ));
            }

            List<PSphere> allValidSpheres = GenerateFromPredefined(candidates, radius, count, rng);

            return allValidSpheres.ToList();
        }

        private static bool IsFarEnoughFromAll(Vector3 candidate, List<PSphere> existingPoints, float sqrRadius)
        {
            foreach (var sphere in existingPoints)
            {
                if ((candidate - sphere.center).sqrMagnitude < sqrRadius)
                {
                    return false;
                }
            }
            return true;
        }

        private static void Shuffle(List<Vector3> list, MT19937 rng)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.NextInt(0, n);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}