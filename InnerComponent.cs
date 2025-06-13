using System;
using UnityEngine;

namespace Corelib.Utils
{
    public class InnerComponent<T> where T : MonoBehaviour
    {
        protected T parent;
        protected Transform transform { get => parent.transform; }
        protected GameObject gameObject { get => parent.gameObject; }

        public InnerComponent<T> SetParent(T parent)
        {
            this.parent = parent;
            return this;
        }

        public virtual void OnEnable()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void OnDrawGizmosSelected()
        {

        }
    }
}