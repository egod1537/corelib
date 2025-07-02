using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutProperty : SUIElement
    {
        private SerializedProperty property;
        private GUIContent guiContent;
        public SEditorGUILayoutProperty(SerializedProperty property)
        {
            this.property = property;
            guiContent = new GUIContent();
        }

        public SEditorGUILayoutProperty GUIContent(GUIContent guiContent)
        {
            this.guiContent = guiContent;
            return this;
        }

        public override void Render()
        {
            EditorGUILayout.PropertyField(property, guiContent, true);
        }
    }
}