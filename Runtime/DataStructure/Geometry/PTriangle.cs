using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using UnityEngine;

namespace Corelib.Utils
{
    [Serializable]
    public class PTriangle : IEnumerable<Vector3>
    {
        public Vector3 v0, v1, v2;
        public Vector3 center { get => (v0 + v1 + v2) / 3.0f; }
        public Vector3 normal;

        public Guid Id { get; private set; }

        public List<Vector3> vertices { get => new() { v0, v1, v2 }; }

        private static readonly Vector3EqualityComparer vertexComparer = new Vector3EqualityComparer(0.0001f);

        public PTriangle(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 normal)
        {
            this.Id = Guid.NewGuid();
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
            this.normal = normal;
        }

        public PTriangle(PTriangle other)
        {
            this.Id = other.Id;
            this.v0 = other.v0;
            this.v1 = other.v1;
            this.v2 = other.v2;
            this.normal = other.normal;
        }

        public bool Equals(PTriangle other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            var thisVertices = new HashSet<Vector3>(new[] { v0, v1, v2 }, vertexComparer);
            var otherVertices = new HashSet<Vector3>(new[] { other.v0, other.v1, other.v2 }, vertexComparer);

            return thisVertices.SetEquals(otherVertices);
        }

        public override bool Equals(object obj)
        {
            return obj is PTriangle other && Equals(other);
        }

        public override int GetHashCode()
        {
            int h0 = vertexComparer.GetHashCode(v0);
            int h1 = vertexComparer.GetHashCode(v1);
            int h2 = vertexComparer.GetHashCode(v2);

            var sortedHashes = new[] { h0, h1, h2 }.OrderBy(h => h).ToArray();

            return (sortedHashes[0], sortedHashes[1], sortedHashes[2]).GetHashCode();
        }

        public void Deconstruct(out Vector3 a, out Vector3 b, out Vector3 c)
        {
            a = v0;
            b = v1;
            c = v2;
        }

        public IEnumerator<Vector3> GetEnumerator()
        {
            yield return v0;
            yield return v1;
            yield return v2;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            return $"PTriangle(\n  v0: {v0},\n  v1: {v1},\n  v2: {v2}\n)";
        }

        public Vector3 ClosestPointOnTriangle(Vector3 point)
        {
            Vector3 a = this.v0;
            Vector3 b = this.v1;
            Vector3 c = this.v2;

            var ab = b - a;
            var ac = c - a;
            var n = Vector3.Cross(ab, ac);

            var p_proj = point - Vector3.Dot(point - a, n) / n.sqrMagnitude * n;

            if (IsPointInTriangleOnSamePlane(p_proj))
            {
                return p_proj;
            }

            var p_ab = ClosestPointOnLineSegment(a, b, point);
            var p_bc = ClosestPointOnLineSegment(b, c, point);
            var p_ca = ClosestPointOnLineSegment(c, a, point);

            var d_ab_sq = (point - p_ab).sqrMagnitude;
            var d_bc_sq = (point - p_bc).sqrMagnitude;
            var d_ca_sq = (point - p_ca).sqrMagnitude;

            float min_d_sq = Mathf.Min(d_ab_sq, d_bc_sq, d_ca_sq);

            if (min_d_sq == d_ab_sq) return p_ab;
            if (min_d_sq == d_bc_sq) return p_bc;
            return p_ca;
        }

        private Vector3 ClosestPointOnLineSegment(Vector3 start, Vector3 end, Vector3 point)
        {
            var segment = end - start;
            float t = Vector3.Dot(point - start, segment) / segment.sqrMagnitude;
            t = Mathf.Clamp01(t);
            return start + segment * t;
        }

        private bool IsPointInTriangleOnSamePlane(Vector3 p)
        {
            var a = this.v0;
            var b = this.v1;
            var c = this.v2;

            var ab = b - a;
            var ac = c - a;
            var ap = p - a;

            float dot_ab_ap = Vector3.Dot(ab, ap);
            float dot_ab_ab = Vector3.Dot(ab, ab);
            float dot_ac_ap = Vector3.Dot(ac, ap);
            float dot_ac_ac = Vector3.Dot(ac, ac);
            float dot_ab_ac = Vector3.Dot(ab, ac);

            float invDenom = 1 / (dot_ab_ab * dot_ac_ac - dot_ab_ac * dot_ab_ac);
            float u = (dot_ac_ac * dot_ab_ap - dot_ab_ac * dot_ac_ap) * invDenom;
            float v = (dot_ab_ab * dot_ac_ap - dot_ab_ac * dot_ab_ap) * invDenom;

            return (u >= 0) && (v >= 0) && (u + v <= 1);
        }

        public static bool IsPointInTriangle(Vector3 p, PTriangle tri)
        {
            var (a, b, c) = (tri.v0, tri.v1, tri.v2);
            Vector3 ab = b - a;
            Vector3 ac = c - a;
            Vector3 ap = p - a;

            float dot_ab_ap = Vector3.Dot(ab, ap);
            float dot_ab_ab = Vector3.Dot(ab, ab);
            float dot_ac_ap = Vector3.Dot(ac, ap);
            float dot_ac_ac = Vector3.Dot(ac, ac);
            float dot_ab_ac = Vector3.Dot(ab, ac);

            float invDenom = 1 / (dot_ab_ab * dot_ac_ac - dot_ab_ac * dot_ab_ac);
            float u = (dot_ac_ac * dot_ab_ap - dot_ab_ac * dot_ac_ap) * invDenom;
            float v = (dot_ab_ab * dot_ac_ap - dot_ab_ac * dot_ab_ap) * invDenom;

            return (u >= 0) && (v >= 0) && (u + v < 1);
        }

        public static bool RayIntersectsTriangle(Ray ray, PTriangle triangle, out float distance)
        {
            const float Epsilon = 1e-8f;
            distance = 0f;

            var (v0, v1, v2) = (triangle.v0, triangle.v1, triangle.v2);
            Vector3 edge1 = v1 - v0;
            Vector3 edge2 = v2 - v0;
            Vector3 h = Vector3.Cross(ray.direction, edge2);
            float a = Vector3.Dot(edge1, h);

            if (a > -Epsilon && a < Epsilon) return false;

            float f = 1.0f / a;
            Vector3 s = ray.origin - v0;
            float u = f * Vector3.Dot(s, h);

            if (u < 0.0f || u > 1.0f) return false;

            Vector3 q = Vector3.Cross(s, edge1);
            float v = f * Vector3.Dot(ray.direction, q);

            if (v < 0.0f || u + v > 1.0f) return false;

            float t = f * Vector3.Dot(edge2, q);
            if (t > Epsilon)
            {
                distance = t;
                return true;
            }

            return false;
        }

        public Vector3 GetRandomPointInside(MT19937 rng = null)
        {
            if (rng == null)
            {
                rng = MT19937.Create();
            }

            float r1 = rng.NextFloat();
            float r2 = rng.NextFloat();

            if (r1 + r2 > 1f)
            {
                r1 = 1f - r1;
                r2 = 1f - r2;
            }

            Vector3 edge1 = v1 - v0;
            Vector3 edge2 = v2 - v0;

            return v0 + r1 * edge1 + r2 * edge2;
        }
    }
}