using UnityEditor;
using UnityEngine;

namespace Corelib.Utils
{
    [CustomPropertyDrawer(typeof(StableEnumAttribute))]
    public class StableEnumDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            var valueProp = prop.FindPropertyRelative("value");
            EditorGUI.PropertyField(pos, valueProp, label, true);
        }
    }
}