
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Corelib.Utils
{
    [InitializeOnLoad]
    public static class HierarchyExtender
    {
        private static Rect rect = new Rect();
        private static Rect selectionRect = new Rect();

        static HierarchyExtender()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            HierarchyExtender.selectionRect = selectionRect;
            rect = new Rect(selectionRect.xMax, selectionRect.y, 0, selectionRect.height);

            Object obj = EditorUtility.InstanceIDToObject(instanceID);
        }
    }
}
