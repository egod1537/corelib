using System;
using UnityEngine;

namespace Corelib.Utils
{
    [Serializable]
    public class FloatRange : IEquatable<FloatRange>, IComparable<FloatRange>
    {
        public float Min;
        public float Max;

        public FloatRange(float min, float max)
        {
            if (min > max)
                throw new ArgumentException("Min must be less than or equal to Max");
            Min = min;
            Max = max;
        }

        public FloatRange(FloatRange other)
        {
            Min = other.Min;
            Max = other.Max;
        }

        public float Length => Max - Min;

        public float Sample(MT19937 rng = null)
            => rng == null ? MT19937.ImNextFloat(Min, Max) : rng.NextFloat(Min, Max);

        public virtual bool Contains(float value)
        {
            return value >= Min && value < Max;
        }

        public bool Intersects(FloatRange other)
        {
            return !(other.Max <= Min || other.Min >= Max);
        }

        public FloatRange? GetIntersection(FloatRange other)
        {
            if (!Intersects(other)) return null;
            float min = Math.Max(Min, other.Min);
            float max = Math.Min(Max, other.Max);
            return new FloatRange(min, max);
        }

        public FloatRange GetUnion(FloatRange other)
        {
            float min = Math.Min(Min, other.Min);
            float max = Math.Max(Max, other.Max);
            return new FloatRange(min, max);
        }

        public override string ToString() => $"[{Min}, {Max}]";

        // --- 비교 로직 추가 ---

        public int CompareTo(FloatRange other)
        {
            if (other is null) return 1;
            int minComparison = Min.CompareTo(other.Min);
            if (minComparison != 0) return minComparison;
            return Max.CompareTo(other.Max);
        }

        public bool Equals(FloatRange other)
        {
            if (other is null) return false;
            return Min.Equals(other.Min) && Max.Equals(other.Max);
        }

        public override bool Equals(object obj)
        {
            return obj is FloatRange other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Min, Max);
        }

        public static bool operator ==(FloatRange left, FloatRange right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(FloatRange left, FloatRange right) => !(left == right);

        public static bool operator <(FloatRange left, FloatRange right)
        {
            if (left is null) return right is not null;
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(FloatRange left, FloatRange right)
        {
            if (left is null) return false;
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(FloatRange left, FloatRange right)
        {
            return !(left > right);
        }

        public static bool operator >=(FloatRange left, FloatRange right)
        {
            return !(left < right);
        }
    }
}