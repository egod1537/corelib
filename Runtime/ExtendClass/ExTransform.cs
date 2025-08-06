using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Corelib.Utils
{
    public static class ExTransform
    {
        public static T GetComponentInDirectChildren<T>(this Transform parent) where T : Component
        {
            foreach (Transform child in parent)
            {
                T component = child.GetComponent<T>();
                if (component != null)
                    return component;
            }
            return null;
        }

        public static List<T> GetComponentsInDirectChildren<T>(this Transform parent) where T : Component
        {
            List<T> components = new List<T>();
            foreach (Transform child in parent)
            {
                T component = child.GetComponent<T>();
                if (component != null)
                    components.Add(component);
            }
            return components;
        }

        public static Matrix4x4 ToMat(this Transform transform)
            => Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);

        public static void DestroyAllChild(this Transform transform)
        {
            int len = transform.childCount;
            for (int i = len - 1; i >= 0; i--)
            {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
        }

        public static void DestroyAllChildrenWithEditor(this Transform transform)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                for (int i = transform.childCount - 1; i >= 0; i--)
                {
                    Transform child = transform.GetChild(i);
                    UnityEditor.Undo.DestroyObjectImmediate(child.gameObject);
                }
                return;
            }
#endif
            // 런타임일 땐 일반 삭제
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
        }

        public static void DestroyImmediateAllChild(this Transform transform)
        {
            int len = transform.childCount;
            for (int i = 0; i < len; i++)
            {
                Object.DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }

        public static List<Transform> Children(this Transform transform)
        {
            List<Transform> children = new();
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                children.Add(child);
                children.AddRange(child.Children());
            }
            return children;
        }

        public static void SetActiveWithChild(this Transform transform, bool active)
        {
            transform.SetActiveChild(active);
            transform.gameObject.SetActive(active);
        }

        public static void SetActiveAllChild(this Transform transform, bool active)
        {
            List<Transform> children = transform.Children();
            foreach (var child in children)
            {
                child.gameObject.SetActive(active);
            }
        }

        public static void SetActiveChild(this Transform transform, bool active)
        {
            foreach (Transform childTransform in transform)
            {
                childTransform.gameObject.SetActive(active);
            }
        }

        public static void SetHideFlagWithChild(this Transform transform, HideFlags hideFlags)
        {
            List<Transform> children = transform.Children();
            foreach (var child in children)
            {
                child.hideFlags = hideFlags;
            }
            transform.hideFlags = hideFlags;
        }

        public static Transform FindInChild(this Transform transform, string name)
            => transform.Cast<Transform>().FirstOrDefault(t => t.name == name);


        public static Transform FindInAllChildren(this Transform root, string name, bool isExcludeRoot = false)
        {
            return root.GetComponentsInChildren<Transform>(true)
                       .FirstOrDefault(t =>
                       {
                           if (isExcludeRoot && t == root) return false;
                           return t.name == name;
                       });
        }

        public static Transform Reset(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            return transform;
        }

        public static Transform ResetLocal(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            return transform;
        }

        public static T GetComponentInParentOnly<T>(this Transform transform) where T : Component
        {
            Transform current = transform.parent;

            while (current != null)
            {
                T component = current.GetComponent<T>();
                if (component != null)
                    return component;
                current = current.parent;
            }

            return null;
        }

        public static T GetComponentInSelfOrParent<T>(this Transform transform) where T : Component
        {
            Transform current = transform;

            while (current != null)
            {
                T component = current.GetComponent<T>();
                if (component != null)
                    return component;
                current = current.parent;
            }

            return null;
        }
    }
}