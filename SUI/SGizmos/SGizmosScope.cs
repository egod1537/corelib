using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SGizmosScope : SUIElement
    {
        private SUIElement element;
        private Matrix4x4? mat;
        private Color? color;

        public SGizmosScope(SUIElement element = null)
        {
            this.element = element;
        }

        public SGizmosScope Matrix(Matrix4x4 mat)
        {
            this.mat = mat;
            return this;
        }
        public SGizmosScope Color(Color color)
        {
            this.color = color;
            return this;
        }

        public override void Render()
        {
            Matrix4x4 prevMat = Gizmos.matrix;
            Color prevColor = Gizmos.color;

            if (mat != null) Gizmos.matrix = mat.Value;
            if (color != null) Gizmos.color = color.Value;

            element?.Render();

            if (mat != null) Gizmos.matrix = prevMat;
            if (color != null) Gizmos.color = prevColor;
        }
    }
}