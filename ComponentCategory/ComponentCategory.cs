using UnityEngine;

namespace Corelib.Utils
{
    public class ComponentCategory : MonoBehaviour
    {
        [SerializeField]
        public ComponentCategoryType selectedCategory;

        [SerializeField]
        public int selectedIdx;
    }
}