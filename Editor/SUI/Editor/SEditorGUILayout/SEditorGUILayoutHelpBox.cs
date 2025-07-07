using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutHelpBox : SUIElement
    {
        private string label;
        private MessageType messageType;
        private Func<bool> where = () => true;

        public SEditorGUILayoutHelpBox()
        {

        }

        public SEditorGUILayoutHelpBox(string label, MessageType messageType)
        {
            this.label = label;
            this.messageType = messageType;
        }

        public SEditorGUILayoutHelpBox Where(Func<bool> callback)
        {
            this.where = callback;
            return this;
        }

        public override void Render()
        {
            if (where())
                EditorGUILayout.HelpBox(label, messageType);
        }
    }
}