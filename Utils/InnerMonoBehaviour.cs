using System.Collections.Generic;
using UnityEngine;
namespace Corelib.Utils
{
    public class InnerMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private List<InnerComponent<T>> innerComponents = new();

        protected InnerComponent<T> AddInnerComponent<P>(T parent) where P : InnerComponent<T>, new()
        {
            InnerComponent<T> newComp = new P().SetParent(parent);
            innerComponents.Add(newComp);
            return newComp;
        }

        protected InnerComponent<T> GetInnerComponent<P>()
        {
            foreach (var comp in innerComponents)
            {
                if (comp is P)
                {
                    return comp;
                }
            }
            return null;
        }

        protected void InnerOnEnable()
        {
            foreach (var comp in innerComponents)
            {
                comp.OnEnable();
            }
        }
        protected void InnerUpdate()
        {
            foreach (var comp in innerComponents)
            {
                comp.Update();
            }
        }

        protected void InnerOnDrawGizmosSelected()
        {
            foreach (var comp in innerComponents)
            {
                comp.OnDrawGizmosSelected();
            }
        }
    }
}