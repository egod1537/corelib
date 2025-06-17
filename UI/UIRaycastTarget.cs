using UnityEngine;
using UnityEngine.UI;

namespace Corelib.Utils
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class UIRaycastTarget : Graphic
    {
        public override void SetMaterialDirty() { return; }
        public override void SetVerticesDirty() { return; }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}