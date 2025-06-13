using System;
using System.Collections.Generic;
using UnityEngine;
namespace Corelib.Utils
{
    public static class ExList
    {
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

        public static T Choice<T>(this List<T> list, MT19937 mt = null)
        {
            if (list.Count == 0)
                return default(T);
            if (mt == null)
                mt = MT19937.Create();

            return list[mt.NextInt(0, list.Count - 1)];
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