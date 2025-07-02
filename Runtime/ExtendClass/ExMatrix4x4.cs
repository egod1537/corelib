using UnityEngine;

namespace Corelib.Utils
{
    public static class ExMatrix4x4
    {
        public static Matrix4x4 ToXY2YZ(this Matrix4x4 mat)
            => new(
                new Vector4(0, 1, 0, 0),
                new Vector4(0, 0, 1, 0),
                new Vector4(0, 0, 0, 0),
                new Vector4(0, 0, 0, 1)
            );

        public static Matrix4x4 ToXY2XZ(this Matrix4x4 mat)
            => new(
                new Vector4(1, 0, 0, 0),
                new Vector4(0, 0, 1, 0),
                new Vector4(0, 0, 0, 0),
                new Vector4(0, 0, 0, 1)
            );
    }
}