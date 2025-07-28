using System;
using System.Collections.Generic;
using UnityEngine;

namespace Corelib.Utils
{
    public class MT19937
    {
        private const int N = 624;
        private const int M = 397;
        private const uint MATRIX_A = 0x9908b0df;
        private const uint UPPER_MASK = 0x80000000;
        private const uint LOWER_MASK = 0x7fffffff;

        private uint[] mt = new uint[N];
        private int mti = N + 1;

        public MT19937(uint seed)
        {
            mt[0] = seed;
            for (mti = 1; mti < N; mti++)
            {
                mt[mti] = (uint)(1812433253U * (mt[mti - 1] ^ (mt[mti - 1] >> 30)) + mti);
            }
        }

        public uint NextUInt()
        {
            uint y;
            uint[] mag01 = { 0x0U, MATRIX_A };

            if (mti >= N)
            {
                int kk;
                for (kk = 0; kk < N - M; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1U];
                }
                for (; kk < N - 1; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + (M - N)] ^ (y >> 1) ^ mag01[y & 0x1U];
                }
                y = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
                mt[N - 1] = mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1U];
                mti = 0;
            }

            y = mt[mti++];

            // Tempering
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680U;
            y ^= (y << 15) & 0xefc60000U;
            y ^= (y >> 18);

            return y;
        }

        public T Choice<T>(IList<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (collection.Count == 0)
            {
                throw new ArgumentException("Collection cannot be empty.", nameof(collection));
            }

            int randomIndex = NextInt(0, collection.Count - 1);
            return collection[randomIndex];
        }

        private int WeightedIndex(IList<float> weights)
        {
            if (weights == null)
                throw new ArgumentNullException(nameof(weights));
            if (weights.Count == 0)
                throw new ArgumentException("weights cannot be empty", nameof(weights));

            float totalWeight = 0.0f;
            for (int i = 0; i < weights.Count; i++)
                totalWeight += weights[i];

            float randomNumber = NextFloat(0, totalWeight);

            for (int i = 0; i < weights.Count; i++)
            {
                if (randomNumber < weights[i])
                    return i;
                randomNumber -= weights[i];
            }

            return weights.Count - 1;
        }

        public T WeightedChoice<T>(IList<T> collection, IList<float> weights)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (weights == null)
                throw new ArgumentNullException(nameof(weights));
            if (collection.Count != weights.Count)
                throw new ArgumentException("weights must have the same size as the collection");

            int idx = WeightedIndex(weights);
            return collection[idx];
        }

        public List<T> WeightedShuffle<T>(IList<T> collection, IList<float> weights)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (weights == null)
                throw new ArgumentNullException(nameof(weights));
            if (collection.Count != weights.Count)
                throw new ArgumentException("weights must have the same size as the collection");

            List<T> items = new(collection);
            List<float> wts = new(weights);
            List<T> result = new(items.Count);

            while (items.Count > 0)
            {
                int idx = WeightedIndex(wts);
                result.Add(items[idx]);
                items.RemoveAt(idx);
                wts.RemoveAt(idx);
            }

            return result;
        }

        public float NextFloat(float min, float max)
        {
            return min + (max - min) * NextFloat();
        }

        public float NextFloat()
        {
            return NextUInt() * (1.0f / 4294967295.0f);
        }

        public Vector3 NextVector3()
        {
            return new Vector3(NextFloat(), NextFloat(), NextFloat());
        }

        public Vector3 NextVector3(Vector3 min, Vector3 max)
        {
            float x = NextFloat(min.x, max.x);
            float y = NextFloat(min.y, max.y);
            float z = NextFloat(min.z, max.z);
            return new Vector3(x, y, z);
        }

        public Quaternion NextQuaternion()
        {
            float u1 = NextFloat();
            float u2 = NextFloat();
            float u3 = NextFloat();

            float sqrt1MinusU1 = Mathf.Sqrt(1f - u1);
            float sqrtU1 = Mathf.Sqrt(u1);

            float twoPiU2 = 2f * Mathf.PI * u2;
            float twoPiU3 = 2f * Mathf.PI * u3;

            float x = sqrt1MinusU1 * Mathf.Sin(twoPiU2);
            float y = sqrt1MinusU1 * Mathf.Cos(twoPiU2);
            float z = sqrtU1 * Mathf.Sin(twoPiU3);
            float w = sqrtU1 * Mathf.Cos(twoPiU3);

            return new Quaternion(x, y, z, w);
        }

        public int NextInt(int min, int max)
        {
            float f = NextFloat();
            float scaled = f * (max - min + 1);
            int rounded = (int)(scaled);
            return min + rounded;
        }

        public static MT19937 Create()
        {
            uint seed = (uint)(DateTime.Now.Ticks & 0xFFFFFFFF);
            return new MT19937(seed);
        }

        public static MT19937 Create(int seed)
        {
            return new MT19937((uint)seed);
        }

        public static float ImNextFloat(float min, float max)
            => Create().NextFloat(min, max);
        public static int ImNextInt(int min, int max)
            => Create().NextInt(min, max);
    }

}
