using System;
using System.Collections.Generic;

namespace Corelib.Utils
{
    public class VoseAlias<T>
    {
        private readonly List<T> values;
        private readonly float[] probability;
        private readonly int[] alias;
        private readonly MT19937 rng;

        public VoseAlias(IList<T> values, IList<float> weights, MT19937 rng = null)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (weights == null)
                throw new ArgumentNullException(nameof(weights));
            if (values.Count != weights.Count)
                throw new ArgumentException("weights must have the same size as values");

            this.values = new List<T>(values);
            this.rng = rng ?? MT19937.Create();
            int n = values.Count;
            probability = new float[n];
            alias = new int[n];

            float[] scaled = new float[n];
            float sum = 0.0f;
            for (int i = 0; i < n; i++)
                sum += weights[i];
            for (int i = 0; i < n; i++)
                scaled[i] = weights[i] * n / sum;

            Queue<int> small = new();
            Queue<int> large = new();
            for (int i = 0; i < n; i++)
            {
                if (scaled[i] < 1.0f)
                    small.Enqueue(i);
                else
                    large.Enqueue(i);
            }

            while (small.Count > 0 && large.Count > 0)
            {
                int l = small.Dequeue();
                int g = large.Dequeue();
                probability[l] = scaled[l];
                alias[l] = g;
                scaled[g] = (scaled[g] + scaled[l]) - 1.0f;
                if (scaled[g] < 1.0f)
                    small.Enqueue(g);
                else
                    large.Enqueue(g);
            }

            while (large.Count > 0)
            {
                int g = large.Dequeue();
                probability[g] = 1.0f;
            }

            while (small.Count > 0)
            {
                int l = small.Dequeue();
                probability[l] = 1.0f;
            }
        }

        public T Next()
        {
            return values[NextIndex()];
        }

        public int NextIndex()
        {
            int i = rng.NextInt(0, probability.Length - 1);
            float r = rng.NextFloat();
            return r < probability[i] ? i : alias[i];
        }
    }
}
