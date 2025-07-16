using UnityEditor;
using UnityEngine;
using Corelib.Utils;
using Corelib.SUI;

[CustomEditor(typeof(PSphereArea))]
public class EditorPSphereArea : Editor
{
    PSphereArea script;

    protected void OnEnable()
    {
        script = (PSphereArea)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var sphere = script.Sphere;

        SEditorGUI.ChangeCheck(target,
            SEditorGUILayout.Float("Radius", script.radius)
            .OnValueChanged(value => script.radius = value)
            + SEditorGUILayout.Color("Color", script.gizmoColor)
            .OnValueChanged(value => script.gizmoColor = value)
            + SEditorGUILayout.Group("World Sphere")
                .Content(
                    SEditorGUI.DisabledGroup(true)
                    .Content(
                        SEditorGUILayout.Vector3("Center", sphere.center)
                        + SEditorGUILayout.Float("Radius", sphere.radius)
                    )
                )
        );

        serializedObject.ApplyModifiedProperties();
    }
}
