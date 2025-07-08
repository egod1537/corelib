using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Corelib.Utils
{
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

        public bool Contains(Vector3 point)
        {
            Vector3 min = Min;
            Vector3 max = Max;
            return (point.x >= min.x && point.x <= max.x) &&
                   (point.y >= min.y && point.y <= max.y) &&
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
    }

    [Serializable]
    public class PBoxInt
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
    }

    public static class PBoxExtensions
    {
        public static PBox FromTriangle(this PTriangle tri)
        {
            var min = Vector3.Min(tri.v0, Vector3.Min(tri.v1, tri.v2));
            var max = Vector3.Max(tri.v0, Vector3.Max(tri.v1, tri.v2));
            return PBox.FromMinMax(min, max);
        }
    }
}