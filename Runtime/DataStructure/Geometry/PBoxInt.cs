using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Corelib.Utils
{
    [Serializable]
    public class PBoxInt : IEnumerable<Vector3Int>
    {
        public Vector3Int topLeft, bottomRight;

        public Vector3Int size { get => bottomRight - topLeft; }
        public int lenX { get => bottomRight.x - topLeft.x; }
        public int lenY { get => bottomRight.y - topLeft.y; }
        public int lenZ { get => bottomRight.z - topLeft.z; }

        public int area { get => (bottomRight - topLeft).Area(); }
        public Vector3 center { get => (Vector3)(topLeft + bottomRight) / 2; }

        public List<PPlane> Faces => Enum
            .GetValues(typeof(PBoxFace))
            .Cast<PBoxFace>()
            .Select(face => GetFace(face))
            .ToList();

        public PBoxInt(Vector3Int topLeft, Vector3Int bottomRight)
        {
            this.topLeft = topLeft;
            this.bottomRight = bottomRight;

            if (!IsValidate())
                throw new ArgumentException($"Invalid cube bounds: topLeft {topLeft} must be less than or equal to bottomRight {bottomRight}.");
        }

        public IEnumerator<Vector3Int> GetEnumerator()
        {
            for (int z = topLeft.z; z < bottomRight.z; z++)
                for (int y = topLeft.y; y < bottomRight.y; y++)
                    for (int x = topLeft.x; x < bottomRight.x; x++)
                        yield return new Vector3Int(x, y, z);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(Vector3 point, float margin)
        {
            Vector3 expandedMin = topLeft - Vector3.one * margin;
            Vector3 expandedMax = bottomRight + Vector3.one * margin;

            return point.x >= expandedMin.x && point.x < expandedMax.x &&
                   point.y >= expandedMin.y && point.y < expandedMax.y &&
                   point.z >= expandedMin.z && point.z < expandedMax.z;
        }

        public bool Contains(Vector3Int point)
        {
            return point.x >= topLeft.x && point.x < bottomRight.x &&
                   point.y >= topLeft.y && point.y < bottomRight.y &&
                   point.z >= topLeft.z && point.z < bottomRight.z;
        }

        public bool Contains(Vector3 point)
        {
            return point.x >= topLeft.x && point.x < bottomRight.x &&
                   point.y >= topLeft.y && point.y < bottomRight.y &&
                   point.z >= topLeft.z && point.z < bottomRight.z;
        }

        public bool ContainsXZ(Vector3 point)
        {
            return point.x >= topLeft.x && point.x < bottomRight.x &&
                   point.z >= topLeft.z && point.z < bottomRight.z;
        }

        public PPlane GetFace(PBoxFace face)
        {
            return face switch
            {
                PBoxFace.LEFT => new PPlane(
                    new Vector3Int(topLeft.x, topLeft.y, topLeft.z),
                    new Vector3Int(topLeft.x, bottomRight.y, bottomRight.z),
                    Vector3Int.left
                ),

                PBoxFace.RIGHT => new PPlane(
                    new Vector3Int(bottomRight.x, topLeft.y, topLeft.z),
                    new Vector3Int(bottomRight.x, bottomRight.y, bottomRight.z),
                    Vector3Int.right
                ),

                PBoxFace.BOTTOM => new PPlane(
                    new Vector3Int(topLeft.x, topLeft.y, topLeft.z),
                    new Vector3Int(bottomRight.x, topLeft.y, bottomRight.z),
                    Vector3Int.down
                ),

                PBoxFace.TOP => new PPlane(
                    new Vector3Int(topLeft.x, bottomRight.y, topLeft.z),
                    new Vector3Int(bottomRight.x, bottomRight.y, bottomRight.z),
                    Vector3Int.up
                ),

                PBoxFace.FRONT => new PPlane(
                    new Vector3Int(topLeft.x, topLeft.y, topLeft.z),
                    new Vector3Int(bottomRight.x, bottomRight.y, topLeft.z),
                    Vector3Int.back
                ),

                PBoxFace.BACK => new PPlane(
                    new Vector3Int(topLeft.x, topLeft.y, bottomRight.z),
                    new Vector3Int(bottomRight.x, bottomRight.y, bottomRight.z),
                    Vector3Int.forward
                ),

                _ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
            };
        }

        private bool IsValidate() => topLeft.LessEqual(bottomRight);

        public bool Overlaps(PBoxInt other)
        {
            if (bottomRight.x < other.topLeft.x || other.bottomRight.x < topLeft.x)
                return false;
            if (bottomRight.y < other.topLeft.y || other.bottomRight.y < topLeft.y)
                return false;
            if (bottomRight.z < other.topLeft.z || other.bottomRight.z < topLeft.z)
                return false;

            return true;
        }

        public bool IsAdjacent(PBoxInt other) => GetAdjacentRegion(other) != null;

        public PBoxInt GetAdjacentRegion(PBoxInt other)
        {
            Vector3Int minA = Vector3Int.Min(this.topLeft, this.bottomRight);
            Vector3Int maxA = Vector3Int.Max(this.topLeft, this.bottomRight);
            Vector3Int minB = Vector3Int.Min(other.topLeft, other.bottomRight);
            Vector3Int maxB = Vector3Int.Max(other.topLeft, other.bottomRight);

            if (maxA.x == minB.x || maxB.x == minA.x)
            {
                int x = (maxA.x == minB.x) ? maxA.x : maxB.x;

                int yMin = Mathf.Max(minA.y, minB.y);
                int yMax = Mathf.Min(maxA.y, maxB.y);
                int zMin = Mathf.Max(minA.z, minB.z);
                int zMax = Mathf.Min(maxA.z, maxB.z);

                if (yMin < yMax && zMin < zMax)
                {
                    return new PBoxInt(
                        new Vector3Int(x, yMin, zMin),
                        new Vector3Int(x, yMax, zMax)
                    );
                }
            }

            if (maxA.y == minB.y || maxB.y == minA.y)
            {
                int y = (maxA.y == minB.y) ? maxA.y : maxB.y;

                int xMin = Mathf.Max(minA.x, minB.x);
                int xMax = Mathf.Min(maxA.x, maxB.x);
                int zMin = Mathf.Max(minA.z, minB.z);
                int zMax = Mathf.Min(maxA.z, maxB.z);

                if (xMin < xMax && zMin < zMax)
                {
                    return new PBoxInt(
                        new Vector3Int(xMin, y, zMin),
                        new Vector3Int(xMax, y, zMax)
                    );
                }
            }

            if (maxA.z == minB.z || maxB.z == minA.z)
            {
                int z = (maxA.z == minB.z) ? maxA.z : maxB.z;

                int xMin = Mathf.Max(minA.x, minB.x);
                int xMax = Mathf.Min(maxA.x, maxB.x);
                int yMin = Mathf.Max(minA.y, minB.y);
                int yMax = Mathf.Min(maxA.y, maxB.y);

                if (xMin < xMax && yMin < yMax)
                {
                    return new PBoxInt(
                        new Vector3Int(xMin, yMin, z),
                        new Vector3Int(xMax, yMax, z)
                    );
                }
            }

            return null;
        }

        public PBoxInt GenerateRandomRoom(MT19937 rng, IList<RoomSpawnInfo> spawnInfos)
        {
            if (spawnInfos == null || spawnInfos.Count == 0)
                throw new ArgumentException("Spawn info list cannot be null or empty.", nameof(spawnInfos));

            var weights = spawnInfos.Select(info => info.weight).ToList();
            var selectedInfo = WeightedChoice(rng, spawnInfos, weights);

            int minSizeX = Mathf.Max(1, (int)(this.lenX * selectedInfo.minSizeMultiplier.x));
            int minSizeY = Mathf.Max(1, (int)(this.lenY * selectedInfo.minSizeMultiplier.y));
            int minSizeZ = Mathf.Max(1, (int)(this.lenZ * selectedInfo.minSizeMultiplier.z));

            int maxSizeX = Mathf.Max(minSizeX, (int)(this.lenX * selectedInfo.maxSizeMultiplier.x));
            int maxSizeY = Mathf.Max(minSizeY, (int)(this.lenY * selectedInfo.maxSizeMultiplier.y));
            int maxSizeZ = Mathf.Max(minSizeZ, (int)(this.lenZ * selectedInfo.maxSizeMultiplier.z));

            int sizeX = rng.NextInt(minSizeX, maxSizeX);
            int sizeY = rng.NextInt(minSizeY, maxSizeY);
            int sizeZ = rng.NextInt(minSizeZ, maxSizeZ);
            var roomSize = new Vector3Int(sizeX, sizeY, sizeZ);

            var positionMargin = this.size - roomSize;
            int offsetX = (positionMargin.x > 0) ? rng.NextInt(0, positionMargin.x) : 0;
            int offsetY = (positionMargin.y > 0) ? rng.NextInt(0, positionMargin.y) : 0;
            int offsetZ = (positionMargin.z > 0) ? rng.NextInt(0, positionMargin.z) : 0;

            var roomTopLeft = this.topLeft + new Vector3Int(offsetX, offsetY, offsetZ);
            var roomBottomRight = roomTopLeft + roomSize;

            return new PBoxInt(roomTopLeft, roomBottomRight);
        }

        public List<Vector3Int> GetRandomPoints(MT19937 rng, int count)
        {
            if (rng == null)
                throw new ArgumentNullException(nameof(rng));
            if (count <= 0)
                return new List<Vector3Int>();

            var points = new List<Vector3Int>(count);
            for (int i = 0; i < count; i++)
            {
                int x = rng.NextInt(topLeft.x, bottomRight.x);
                int y = rng.NextInt(topLeft.y, bottomRight.y);
                int z = rng.NextInt(topLeft.z, bottomRight.z);
                points.Add(new Vector3Int(x, y, z));
            }
            return points;
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

        public bool Contains(PBoxInt other)
        {
            return topLeft.x <= other.topLeft.x && bottomRight.x >= other.bottomRight.x &&
                   topLeft.y <= other.topLeft.y && bottomRight.y >= other.bottomRight.y &&
                   topLeft.z <= other.topLeft.z && bottomRight.z >= other.bottomRight.z;
        }

        public static PBoxInt Intersection(PBoxInt a, PBoxInt b)
        {
            Vector3Int tl = Vector3Int.Max(a.topLeft, b.topLeft);
            Vector3Int br = Vector3Int.Min(a.bottomRight, b.bottomRight);
            if (tl.x >= br.x || tl.y >= br.y || tl.z >= br.z)
                return null;
            return new PBoxInt(tl, br);
        }

        public static List<PBoxInt> Subtract(PBoxInt a, PBoxInt b)
        {
            var result = new List<PBoxInt>();

            if (!a.Overlaps(b))
            {
                result.Add(new PBoxInt(a.topLeft, a.bottomRight));
                return result;
            }

            if (b.Contains(a))
                return result;

            var xCoords = new List<int> { a.topLeft.x, a.bottomRight.x };
            if (b.topLeft.x > a.topLeft.x && b.topLeft.x < a.bottomRight.x) xCoords.Add(b.topLeft.x);
            if (b.bottomRight.x > a.topLeft.x && b.bottomRight.x < a.bottomRight.x) xCoords.Add(b.bottomRight.x);

            var yCoords = new List<int> { a.topLeft.y, a.bottomRight.y };
            if (b.topLeft.y > a.topLeft.y && b.topLeft.y < a.bottomRight.y) yCoords.Add(b.topLeft.y);
            if (b.bottomRight.y > a.topLeft.y && b.bottomRight.y < a.bottomRight.y) yCoords.Add(b.bottomRight.y);

            var zCoords = new List<int> { a.topLeft.z, a.bottomRight.z };
            if (b.topLeft.z > a.topLeft.z && b.topLeft.z < a.bottomRight.z) zCoords.Add(b.topLeft.z);
            if (b.bottomRight.z > a.topLeft.z && b.bottomRight.z < a.bottomRight.z) zCoords.Add(b.bottomRight.z);

            xCoords = xCoords.Distinct().OrderBy(x => x).ToList();
            yCoords = yCoords.Distinct().OrderBy(y => y).ToList();
            zCoords = zCoords.Distinct().OrderBy(z => z).ToList();

            for (int i = 0; i < xCoords.Count - 1; i++)
            {
                for (int j = 0; j < yCoords.Count - 1; j++)
                {
                    for (int k = 0; k < zCoords.Count - 1; k++)
                    {
                        var min = new Vector3Int(xCoords[i], yCoords[j], zCoords[k]);
                        var max = new Vector3Int(xCoords[i + 1], yCoords[j + 1], zCoords[k + 1]);
                        var subBox = new PBoxInt(min, max);

                        if (!b.Contains(subBox.center))
                        {
                            result.Add(subBox);
                        }
                    }
                }
            }

            return result;
        }
    }
}