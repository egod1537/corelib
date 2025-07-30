using System.Collections.Generic;
using UnityEngine;
namespace Corelib.Utils
{
    public static class ExGameObject
    {
        public static T MaybeAddComponent<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        public static void SetActiveWithChild(this GameObject gameObject, bool active)
        {
            gameObject.transform.SetActiveWithChild(active);
        }

        public static void SetHideFlagWithChild(this GameObject gameObject, HideFlags hideFlags)
        {
            gameObject.transform.SetHideFlagWithChild(hideFlags);
        }

        public static bool HasComponent<T>(this GameObject gameObject) where T : MonoBehaviour
            => gameObject.GetComponent<T>() != null;

        public static T GetComponentInParentOnly<T>(this GameObject gameObject) where T : Component
            => gameObject.transform.GetComponentInParentOnly<T>();

        public static T GetComponentInSelfOrParent<T>(this GameObject gameObject) where T : Component
            => gameObject.transform.GetComponentInSelfOrParent<T>();

        public static void SafeDestroy(this GameObject go)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                Object.Destroy(go);
            }
            else
            {
                Object.DestroyImmediate(go);
            }
#else
        Object.Destroy(go);
#endif
        }

        public static void SafeDestroyAllChild(this GameObject go)
        {
            Transform tr = go.transform;
            for (int i = tr.childCount - 1; i >= 0; i--)
            {
                tr.GetChild(i).gameObject.SafeDestroy();
            }
        }

        public static void SetLayerRecursively(this GameObject gameObject, int newLayer)
        {
            if (gameObject == null) return;

            gameObject.layer = newLayer;

            foreach (Transform child in gameObject.transform)
                SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}