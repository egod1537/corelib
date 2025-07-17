using System;
using System.Reflection;
using UnityEditor;

namespace Corelib.Utils
{
    public static class SerializedPropertyExtensions
    {
        public static T GetValue<T>(this SerializedProperty property)
        {
            object obj = property.serializedObject.targetObject;
            string path = property.propertyPath.Replace(".Array.data[", "[");
            string[] fieldNames = path.Split('.');

            foreach (var fieldName in fieldNames)
            {
                if (fieldName.Contains("["))
                {
                    int index = Convert.ToInt32(fieldName.Substring(fieldName.IndexOf("[", StringComparison.Ordinal) + 1,
                        fieldName.Length - fieldName.IndexOf("[", StringComparison.Ordinal) - 2));
                    var fieldInfo = obj.GetType().GetField(fieldName.Substring(0, fieldName.IndexOf("[", StringComparison.Ordinal)),
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    obj = ((System.Collections.IList)fieldInfo.GetValue(obj))[index];
                }
                else
                {
                    var fieldInfo = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    obj = fieldInfo.GetValue(obj);
                }
            }
            return (T)obj;
        }
    }
}