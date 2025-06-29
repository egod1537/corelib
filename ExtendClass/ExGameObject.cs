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
    }
}