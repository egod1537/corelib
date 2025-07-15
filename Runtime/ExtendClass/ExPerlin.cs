using UnityEngine;

namespace Corelib.Utils
{
    public static class ExPerlin
    {
        private static readonly int[] p = {
        151,160,137,91,90,15,131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
        190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,88,237,149,56,87,174,
        20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,77,146,158,231,83,111,229,122,60,211,133,
        230,220,105,92,41,55,46,245,40,244,102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,
        18,169,200,196,135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,5,202,
        38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,223,183,170,213,119,248,
        152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,129,22,39,253, 19,98,108,110,79,113,224,232,
        178,185, 112,104,218,246,97,228,251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,
        14,239,107,49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,138,236,
        205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
    };

        private static readonly int[] permutation;

        static ExPerlin()
        {
            permutation = new int[512];
            for (int i = 0; i < 256; i++)
            {
                permutation[i] = permutation[i + 256] = p[i];
            }
        }

        public static float Noise(float x, float y, float z, MT19937 rng = null)
            => Noise(x + rng.NextFloat(), y + rng.NextFloat(), z + rng.NextFloat());
        public static float Noise(float x, float y, float z, Vector3 seed)
            => Noise(x + seed.x, y + seed.y, z + seed.z);
        public static float Noise(Vector3 position, Vector3 seed)
            => Noise(position.x + seed.x, position.y + seed.y, position.z + seed.z);

        public static float Noise(float x, float y, float z)
        {
            int xi = Mathf.FloorToInt(x) & 255;
            int yi = Mathf.FloorToInt(y) & 255;
            int zi = Mathf.FloorToInt(z) & 255;

            float xf = x - Mathf.Floor(x);
            float yf = y - Mathf.Floor(y);
            float zf = z - Mathf.Floor(z);

            float u = Fade(xf);
            float v = Fade(yf);
            float w = Fade(zf);

            int aaa = permutation[permutation[permutation[xi] + yi] + zi];
            int aba = permutation[permutation[permutation[xi] + yi + 1] + zi];
            int aab = permutation[permutation[permutation[xi] + yi] + zi + 1];
            int abb = permutation[permutation[permutation[xi] + yi + 1] + zi + 1];
            int baa = permutation[permutation[permutation[xi + 1] + yi] + zi];
            int bba = permutation[permutation[permutation[xi + 1] + yi + 1] + zi];
            int bab = permutation[permutation[permutation[xi + 1] + yi] + zi + 1];
            int bbb = permutation[permutation[permutation[xi + 1] + yi + 1] + zi + 1];

            float x1, x2, y1, y2;
            x1 = Mathf.Lerp(Grad(aaa, xf, yf, zf), Grad(baa, xf - 1, yf, zf), u);
            x2 = Mathf.Lerp(Grad(aba, xf, yf - 1, zf), Grad(bba, xf - 1, yf - 1, zf), u);
            y1 = Mathf.Lerp(x1, x2, v);

            x1 = Mathf.Lerp(Grad(aab, xf, yf, zf - 1), Grad(bab, xf - 1, yf, zf - 1), u);
            x2 = Mathf.Lerp(Grad(abb, xf, yf - 1, zf - 1), Grad(bbb, xf - 1, yf - 1, zf - 1), u);
            y2 = Mathf.Lerp(x1, x2, v);

            return (Mathf.Lerp(y1, y2, w) + 1) / 2;
        }

        private static float Fade(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private static float Grad(int hash, float x, float y, float z)
        {
            int h = hash & 15;
            float u = h < 8 ? x : y;
            float v = h < 4 ? y : h == 12 || h == 14 ? x : z;
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }
    }
}