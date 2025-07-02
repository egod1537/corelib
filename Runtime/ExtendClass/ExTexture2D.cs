using UnityEngine;
namespace Corelib.Utils
{
    public static class ExTexture2D
    {
        public static Texture2D ResizeTexture(this Texture2D texture2D, int newWidth, int newHeight)
        {
            RenderTexture rt = new RenderTexture(newWidth, newHeight, 0);
            Graphics.Blit(texture2D, rt);
            Texture2D newTexture = new Texture2D(newWidth, newHeight);
            newTexture.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
            newTexture.Apply();
            RenderTexture.active = null;
            rt.Release();
            return newTexture;
        }
    }
}