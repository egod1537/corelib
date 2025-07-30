using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Corelib.Utils
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class UIAutoHeightFromChildren : MonoBehaviour
    {
        [Header("Height를 합산할 자식들")]
        [SerializeField]
        private RectTransform[] targetChildren;

        private RectTransform _self;

        private void Awake()
        {
            _self = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _self = GetComponent<RectTransform>();
        }

        [Button("Resize (Apply Children Height)")]
        public void Resize()
        {
            if (_self == null)
                _self = GetComponent<RectTransform>();

            float total = 0f;
            foreach (var child in targetChildren)
            {
                if (child == null) continue;

                LayoutRebuilder.ForceRebuildLayoutImmediate(child);
                total += child.rect.height;
            }

            _self.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, total);

            if (_self.parent is RectTransform parent)
                LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
        }
    }
}
