using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Corelib.Utils
{
    using Category = ComponentCategoryType;

    public partial class EditorTransform : InnerEditor<Transform>
    {
        private class ComponentCategoryGUI : InnerGUI<Transform>
        {
            private ComponentCategory componentCategory;

            public override void OnEnable()
            {
                base.OnEnable();
                componentCategory = script.GetComponent<ComponentCategory>();
            }

            public override void OnInspectorGUI()
            {
                bool isUpdate = false;

                EditorGUIDrawer.DrawInspectorLine();
                EditorGUILayout.BeginHorizontal("Box");
                {
                    List<Category> categories = script.gameObject.GetComponentCategories();
                    string[] categoryNames = categories.Select(c => c.ToString()).ToArray();

                    int nowSelected =
                         GUILayout.Toolbar(componentCategory.selectedIdx, categoryNames);
                    if (nowSelected != componentCategory.selectedIdx)
                    {
                        isUpdate = true;
                    }

                    componentCategory.selectedIdx = nowSelected;
                    componentCategory.selectedCategory = categories[nowSelected];
                }
                EditorGUILayout.EndHorizontal();

                FilterCategoryComponents(componentCategory.selectedCategory);
                if (isUpdate)
                {
                    EditorUtility.SetDirty(script);
                }
            }

            private void FilterCategoryComponents(Category category)
            {
                SetHideAllCategoryComponent(true);
                ShowCategoryComponent(category);
            }

            private void SetHideAllCategoryComponent(bool hide)
            {
                Component[] components = script.GetComponents<Component>();
                foreach (Component component in components)
                {
                    if (component is Transform || component is ComponentCategory)
                    {
                        continue;
                    }

                    component.hideFlags = hide ? HideFlags.HideInInspector : HideFlags.None;
                }
            }

            private void ShowCategoryComponent(Category category)
            {
                Component[] components = script.GetComponents<Component>();
                foreach (Component component in components)
                {
                    if (!ComponentCategoryFactory.InCategory(category, component))
                    {
                        continue;
                    }
                    component.hideFlags = HideFlags.None;
                }
            }
        }
    }
}