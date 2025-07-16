using System;
using UnityEngine;

namespace Corelib.Utils
{
    [Serializable]
    public class IntRange : IEquatable<IntRange>
    {
        public int Min;
        public int Max;

        public IntRange(int min, int max)
        {
            if (min > max)
                throw new ArgumentException("Min must be less than or equal to Max");
            Min = min;
            Max = max;
        }

        public IntRange(IntRange other)
        {
            this.Min = other.Min;
            this.Max = other.Max;
        }

        public int Length => Max - Min;

        public int Sample(MT19937 rng = null, bool isInclude = false)
            => rng == null ? MT19937.ImNextInt(Min, Max - (isInclude ? 0 : 1)) : rng.NextInt(Min, Max - (isInclude ? 0 : 1));

        public bool Contains(int value)
        {
            return value >= Min && value <= Max;
        }

        public bool Intersects(IntRange other)
        {
            return !(other.Max < Min || other.Min > Max);
        }

        public IntRange? GetIntersection(IntRange other)
        {
            if (!Intersects(other)) return null;
            int min = Math.Max(Min, other.Min);
            int max = Math.Min(Max, other.Max);
            return new IntRange(min, max);
        }

        public IntRange GetUnion(IntRange other)
        {
            int min = Math.Min(Min, other.Min);
            int max = Math.Max(Max, other.Max);
            return new IntRange(min, max);
        }

        public override string ToString() => $"[{Min}, {Max}]";

        public bool Equals(IntRange other)
        {
            if (other is null) return false;
            return Min.Equals(other.Min) && Max.Equals(other.Max);
        }

        public override bool Equals(object obj)
        {
            return obj is IntRange other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Min, Max);
        }

        public static bool operator ==(IntRange left, IntRange right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(IntRange left, IntRange right) => !(left == right);
    }
}