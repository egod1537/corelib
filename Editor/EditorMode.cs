using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

namespace Corelib.Utils
{
    public static class EditorMode
    {
        public static List<GameObject> GetRootObjects()
            => SceneManager.GetActiveScene().GetRootGameObjects().ToList();
        public static void SetActiveAll(bool active)
        {
            List<GameObject> rootObjects = GetRootObjects();
            foreach (var go in rootObjects)
            {
                go.SetActive(active);
            }
        }
        public static void SetHideAll(HideFlags hideFlags)
        {
            List<GameObject> rootObjects = GetRootObjects();
            foreach (var go in rootObjects)
            {
                go.hideFlags = hideFlags;
            }
        }
        public static void SetActiveExcept(Transform transform, bool active)
        {
            SetHideAll(active ? HideFlags.HideInHierarchy : HideFlags.None);
            if (active)
            {
                transform.SetHideFlagWithChild(HideFlags.None);
            }
        }
    }
}