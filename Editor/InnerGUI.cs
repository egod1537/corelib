using UnityEngine;
using UnityEditor;
namespace Corelib.Utils
{
    public class InnerGUI<T> where T : Object
    {
        protected T script;
        protected InnerEditor<T> editor;

        public InnerGUI()
        {

        }

        public InnerGUI<T> SetScript(T script)
        {
            this.script = script;
            return this;
        }

        public InnerGUI<T> SetEditor(InnerEditor<T> editor)
        {
            this.editor = editor;
            return this;
        }

        public P GetInnerGUI<P>() where P : InnerGUI<T>
        {
            return editor.GetInnerGUI<P>();
        }

        public virtual void OnEnable()
        {

        }

        public virtual void OnDisable()
        {

        }

        public virtual void OnInspectorGUI()
        {

        }

        public virtual void OnGUI()
        {

        }

        public virtual void OnSceneGUI(SceneView sceneView)
        {

        }
    }
}