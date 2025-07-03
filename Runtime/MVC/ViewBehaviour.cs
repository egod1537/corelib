using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Corelib.Utils
{
    public abstract class ViewBehaviour : MonoBehaviour
    {
        [HideInInspector]
        public ViewBehaviour parentView;
        [HideInInspector]
        public List<ViewBehaviour> childViews;

        protected T GetChildView<T>() where T : ViewBehaviour
            => (T)childViews.First(child => child is T);

        protected void Awake()
        {
            InitializeViewComponent();
        }

        private void InitializeViewComponent()
        {
            Transform tr = transform.parent;
            while (tr != null)
            {
                ViewBehaviour ui = tr.GetComponent<ViewBehaviour>();
                if (ui != null)
                {
                    ui.childViews.Add(this);
                    parentView = ui;
                    break;
                }
                tr = tr.parent;
            }
        }
        public abstract void Render();

        public List<T> FindAllChild<T>() where T : ViewBehaviour
        {
            List<T> childs = new();
            if (this is T) childs.Add(this as T);

            foreach (var child in childViews)
                childs.AddRange(child.FindAllChild<T>());
            return childs;
        }
    }
}