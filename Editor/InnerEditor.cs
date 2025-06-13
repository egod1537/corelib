using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.Utils
{
    public class InnerEditor<T> : Editor where T : Object
    {
        protected T script;
        protected List<InnerGUI<T>> innerGUIs;

        public virtual void OnEnable()
        {
            script = (T)target;
            innerGUIs = new();
        }

        public virtual void OnDisable()
        {

        }

        public void OnEnableInnerGUI()
        {
            foreach (var innerGUI in innerGUIs)
            {
                innerGUI.OnEnable();
                SceneView.duringSceneGui += innerGUI.OnSceneGUI;
            }
        }

        public virtual void OnDisableInnerGUI()
        {
            foreach (var innerGUI in innerGUIs)
            {
                innerGUI.OnDisable();
                SceneView.duringSceneGui -= innerGUI.OnSceneGUI;

            }
        }

        public void OnInspectorInnerGUI()
        {
            foreach (var innerGUI in innerGUIs)
            {
                innerGUI.OnInspectorGUI();
            }
        }

        public void OnGUIInnerGUI()
        {
            foreach (var innerGUI in innerGUIs)
            {
                innerGUI.OnGUI();
            }
        }

        public P GetInnerGUI<P>() where P : InnerGUI<T>
        {
            return innerGUIs.Find(innerGUI => innerGUI is P) as P;
        }

        protected InnerGUI<T> AddInnerGUI<P>() where P : InnerGUI<T>, new()
        {
            InnerGUI<T> newGUI = new P().SetScript(script).SetEditor(this);
            innerGUIs.Add(newGUI);
            newGUI.OnEnable();
            return newGUI;
        }
    }
}