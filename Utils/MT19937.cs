using System;
using System.Collections.Generic;

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

            int randomIndex = NextInt(0, collection.Count);
            return collection[randomIndex];
        }

        public float NextFloat(float min, float max)
        {
            return min + (max - min) * NextFloat();
        }

        public float NextFloat()
        {
            return NextUInt() * (1.0f / 4294967295.0f);
        }

        public int NextInt(int min, int max)
        {
            return min + (int)(NextFloat() * (max - min));
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
    }

}
