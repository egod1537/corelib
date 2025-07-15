using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Corelib.Utils
{
    public class RoomSpawnInfo
    {
        public float weight;
        public Vector3 minSizeMultiplier;
        public Vector3 maxSizeMultiplier;
    }

    public enum PBoxFace
    {
        FRONT,
        BACK,
        TOP,
        BOTTOM,
        LEFT,
        RIGHT,
    }

    public class PBox
    {
        public Vector3 center;
        public Vector3 size;

        public Vector3 Min => center - size * 0.5f;
        public Vector3 Max => center + size * 0.5f;

        public PBox(Vector3 center, Vector3 size)
        {
            this.center = center;
            this.size = size;
        }

        public static PBox FromMinMax(Vector3 min, Vector3 max)
        {
            var size = max - min;
            var center = min + size * 0.5f;
            return new PBox(center, size);
        }

        public static PBox FromTriangle(PTriangle tri)
        {
            var min = Vector3.Min(tri.v0, Vector3.Min(tri.v1, tri.v2));
            var max = Vector3.Max(tri.v0, Vector3.Max(tri.v1, tri.v2));
            return FromMinMax(min, max);
        }

        public bool Contains(Vector3 point, float distance)
        {
            Vector3 expandedMin = Min - Vector3.one * distance;
            Vector3 expandedMax = Max + Vector3.one * distance;

            return (point.x >= expandedMin.x && point.x <= expandedMax.x) &&
                   (point.y >= expandedMin.y && point.y <= expandedMax.y) &&
                   (point.z >= expandedMin.z && point.z <= expandedMax.z);
        }

        public bool Contains(Vector3 point)
        {
            Vector3 min = Min;
            Vector3 max = Max;
            return (point.x >= min.x && point.x <= max.x) &&
                   (point.y >= min.y && point.y <= max.y) &&
                   (point.z >= min.z && point.z <= max.z);
        }

        public bool ContainsXZ(Vector3 point)
        {
            Vector3 min = Min;
            Vector3 max = Max;
            return (point.x >= min.x && point.x <= max.x) &&
                   (point.z >= min.z && point.z <= max.z);
        }

        public bool Intersects(PBox other)
        {
            Vector3 aMin = Min;
            Vector3 aMax = Max;
            Vector3 bMin = other.Min;
            Vector3 bMax = other.Max;

            return (aMin.x <= bMax.x && aMax.x >= bMin.x) &&
                   (aMin.y <= bMax.y && aMax.y >= bMin.y) &&
                   (aMin.z <= bMax.z && aMax.z >= bMin.z);
        }

        public bool Intersects(Ray ray, out float distance)
        {
            return ToBounds().IntersectRay(ray, out distance);
        }

        public float Volume => size.x * size.y * size.z;

        public Bounds ToBounds() => new Bounds(center, size);

        public static bool DoBoxesIntersect(PBox a, PBox b)
        {
            return (a.Min.x <= b.Max.x && a.Max.x >= b.Min.x) &&
                   (a.Min.y <= b.Max.y && a.Max.y >= b.Min.y) &&
                   (a.Min.z <= b.Max.z && a.Max.z >= b.Min.z);
        }

        public List<PBox> Subtract(PBox other)
        {
            var result = new List<PBox>();

            if (!this.Intersects(other))
            {
                result.Add(this);
                return result;
            }

            if (other.Contains(this.Min) && other.Contains(this.Max))
            {
                return result;
            }

            var xCoords = new List<float> { this.Min.x, this.Max.x };
            if (other.Min.x > this.Min.x && other.Min.x < this.Max.x) xCoords.Add(other.Min.x);
            if (other.Max.x > this.Min.x && other.Max.x < this.Max.x) xCoords.Add(other.Max.x);

            var yCoords = new List<float> { this.Min.y, this.Max.y };
            if (other.Min.y > this.Min.y && other.Min.y < this.Max.y) yCoords.Add(other.Min.y);
            if (other.Max.y > this.Min.y && other.Max.y < this.Max.y) yCoords.Add(other.Max.y);

            var zCoords = new List<float> { this.Min.z, this.Max.z };
            if (other.Min.z > this.Min.z && other.Min.z < this.Max.z) zCoords.Add(other.Min.z);
            if (other.Max.z > this.Min.z && other.Max.z < this.Max.z) zCoords.Add(other.Max.z);

            xCoords = xCoords.Distinct().OrderBy(x => x).ToList();
            yCoords = yCoords.Distinct().OrderBy(y => y).ToList();
            zCoords = zCoords.Distinct().OrderBy(z => z).ToList();

            for (int i = 0; i < xCoords.Count - 1; i++)
            {
                for (int j = 0; j < yCoords.Count - 1; j++)
                {
                    for (int k = 0; k < zCoords.Count - 1; k++)
                    {
                        var min = new Vector3(xCoords[i], yCoords[j], zCoords[k]);
                        var max = new Vector3(xCoords[i + 1], yCoords[j + 1], zCoords[k + 1]);
                        var subBox = PBox.FromMinMax(min, max);

                        if (!other.Contains(subBox.center))
                        {
                            result.Add(subBox);
                        }
                    }
                }
            }
            return result;
        }

        public PBox GenerateRandomRoom(MT19937 rng, IList<RoomSpawnInfo> spawnInfos)
        {
            if (spawnInfos == null || spawnInfos.Count == 0)
                throw new ArgumentException("Spawn info list cannot be null or empty.", nameof(spawnInfos));

            var weights = spawnInfos.Select(info => info.weight).ToList();
            var selectedInfo = WeightedChoice(rng, spawnInfos, weights);

            float sizeX = rng.NextFloat(size.x * selectedInfo.minSizeMultiplier.x, size.x * selectedInfo.maxSizeMultiplier.x);
            float sizeY = rng.NextFloat(size.y * selectedInfo.minSizeMultiplier.y, size.y * selectedInfo.maxSizeMultiplier.y);
            float sizeZ = rng.NextFloat(size.z * selectedInfo.minSizeMultiplier.z, size.z * selectedInfo.maxSizeMultiplier.z);
            var roomSize = new Vector3(sizeX, sizeY, sizeZ);

            var positionMargin = this.size - roomSize;
            float offsetX = rng.NextFloat(-positionMargin.x / 2f, positionMargin.x / 2f);
            float offsetY = rng.NextFloat(-positionMargin.y / 2f, positionMargin.y / 2f);
            float offsetZ = rng.NextFloat(-positionMargin.z / 2f, positionMargin.z / 2f);
            var roomCenter = this.center + new Vector3(offsetX, offsetY, offsetZ);

            return new PBox(roomCenter, roomSize);
        }

        private static T WeightedChoice<T>(MT19937 rng, IList<T> collection, IList<float> weights)
        {
            float totalWeight = weights.Sum();
            float randomNumber = rng.NextFloat(0, totalWeight);

            for (int i = 0; i < collection.Count; i++)
            {
                if (randomNumber < weights[i])
                    return collection[i];
                randomNumber -= weights[i];
            }
            return collection[collection.Count - 1];
        }
    }
}