using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Corelib.Utils
{
    public static class ExList
    {
        public static T Front<T>(this List<T> list) => list[0];
        public static T Back<T>(this List<T> list) => list[list.Count - 1];

        public static void Swap<T>(this List<T> list, int idx1, int idx2)
        {
            (list[idx1], list[idx2]) = (list[idx2], list[idx1]);
        }

        public static List<T> Resize<T>(this List<T> list, int size, T value)
        {
            List<T> copy = new List<T>(list);
            list.Clear();
            for (int i = 0; i < size; i++)
            {
                if (i < copy.Count)
                {
                    list.Add(copy[i]);
                }
                else
                {
                    list.Add(value);
                }
            }
            return list;
        }

        public static List<T> Resize<T>(this List<T> list, int size)
            => list.Resize(size, default);

        public static T Choice<T>(this List<T> list)
        {
            if (list.Count == 0)
                return default(T);

            return list.Choice(MT19937.Create());
        }

        public static T Choice<T>(this List<T> list, MT19937 rng = null)
        {
            if (list.Count == 0)
                return default(T);
            if (rng == null)
                rng = MT19937.Create();

            return list[rng.NextInt(0, list.Count - 1)];
        }

        public static T Choice<T>(this List<T> list, MT19937 rng, List<float> weights)
        {
            if (list.Count == 0)
                return default(T);
            if (weights == null || weights.Count != list.Count)
                throw new ArgumentException("weights must have the same size as the list");

            var totalWeight = weights.Sum();
            var randomNumber = rng.NextFloat(0, totalWeight);

            for (int i = 0; i < list.Count; i++)
            {
                if (randomNumber < weights[i])
                {
                    return list[i];
                }
                randomNumber -= weights[i];
            }

            return list.Back();
        }

        public static List<T> Shuffle<T>(this List<T> list, MT19937 rng = null)
        {
            rng ??= MT19937.Create();
            for (int i = 1; i < list.Count; i++)
            {
                int j = rng.NextInt(0, i - 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
            return list;
        }
    }
}