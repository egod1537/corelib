using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(AutoCreateScriptableObjectAttribute))]
public class AutoCreateScriptableObjectDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float buttonWidth = 64f;
        Rect fieldRect = new Rect(position.x, position.y, position.width - buttonWidth - 5f, position.height);
        Rect buttonRect = new Rect(fieldRect.xMax + 5f, position.y, buttonWidth, position.height);

        EditorGUI.PropertyField(fieldRect, property, label);
        bool hasSO = property.objectReferenceValue != null;

        EditorGUI.BeginDisabledGroup(hasSO);
        if (GUI.Button(buttonRect, "Create"))
        {
            var attr = attribute as AutoCreateScriptableObjectAttribute;
            string folder = attr.assetPath;
            string soTypeName = fieldInfo.FieldType.Name;

            if (!AssetDatabase.IsValidFolder(folder))
                AssetDatabase.CreateFolder("Assets", folder.Replace("Assets/", ""));

            var soType = fieldInfo.FieldType;
            var instance = ScriptableObject.CreateInstance(soType);
            string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{folder}/{soTypeName}.asset");

            AssetDatabase.CreateAsset(instance, assetPath);
            AssetDatabase.SaveAssets();

            property.objectReferenceValue = instance;
            property.serializedObject.ApplyModifiedProperties();

            Debug.Log($"[{soTypeName}] 생성 완료: {assetPath}");
        }
        EditorGUI.EndDisabledGroup();

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}
