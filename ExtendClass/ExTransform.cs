using System.Collections.Generic;
using UnityEngine;
namespace Corelib.Utils
{
    public static class ExTransform
    {
        public static Matrix4x4 ToMat(this Transform transform)
            => Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);

        public static void DestroyAllChild(this Transform transform)
        {
            int len = transform.childCount;
            for (int i = 0; i < len; i++)
            {
                Object.Destroy(transform.GetChild(0).gameObject);
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
            List<Transform> children = transform.Children();
            foreach (var child in children)
            {
                child.gameObject.SetActive(active);
            }
            transform.gameObject.SetActive(active);
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
    }
}