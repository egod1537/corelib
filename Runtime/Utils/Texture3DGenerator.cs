using System;
using UnityEngine;

namespace Corelib.Utils
{
    public static class Texture3DGenerator
    {
        public static Texture3D GenerateTexture3D(Vector3Int size, Func<Vector3Int, Color32> colorFunc,
            FilterMode filter = FilterMode.Bilinear,
            TextureWrapMode wrap = TextureWrapMode.Clamp,
            string name = "GeneratedTexture3D")
        {
            int width = size.x;
            int height = size.y;
            int depth = size.z;

            var tex = new Texture3D(width, height, depth, TextureFormat.RGBA32, false)
            {
                wrapMode = wrap,
                filterMode = filter,
                name = name
            };

            var pixels = new Color32[width * height * depth];

            for (int z = 0; z < depth; z++)
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        int idx = x + y * width + z * width * height;
                        pixels[idx] = colorFunc(new Vector3Int(x, y, z));
                    }

            tex.SetPixels32(pixels);
            tex.Apply(false, true);
            return tex;
        }
    }
}

