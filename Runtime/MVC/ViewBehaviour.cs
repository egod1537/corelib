using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Corelib.Utils
{
    public abstract class ViewBehaviour<R> : MonoBehaviour where R : ViewBaseBehaviour
    {
        [HideInInspector]
        public R rootView;
        [HideInInspector]
        public ViewBehaviour<R> parentView;
        [HideInInspector]
        public List<ViewBehaviour<R>> childViews = new List<ViewBehaviour<R>>();

        protected T GetChildView<T>() where T : ViewBehaviour<R>
        {
            return (T)childViews.FirstOrDefault(child => child is T);
        }

        protected virtual void Awake()
        {
            InitializeViewComponent();
        }

        private void InitializeViewComponent()
        {
            rootView = GetComponentInParent<R>();

            parentView = GetParentView();
            if (parentView != null)
            {
                parentView.childViews.Add(this);
            }
        }

        private ViewBehaviour<R> GetParentView()
        {
            Transform parentTransform = transform.parent;
            while (parentTransform != null)
            {
                var parent = parentTransform.GetComponent<ViewBehaviour<R>>();
                if (parent != null)
                {
                    return parent;
                }
                parentTransform = parentTransform.parent;
            }
            return null;
        }

        public List<T> FindAllChild<T>() where T : ViewBehaviour<R>
        {
            List<T> childs = new List<T>();
            if (this is T)
            {
                childs.Add(this as T);
            }

            foreach (var child in childViews)
            {
                childs.AddRange(child.FindAllChild<T>());
            }
            return childs;
        }
    }
}