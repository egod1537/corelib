using UnityEditor;
using UnityEngine;
using Corelib.Utils;
using Corelib.SUI;

[CustomEditor(typeof(PCapsuleArea))]
public class EditorPCapsuleArea : Editor
{
    PCapsuleArea script;

    protected void OnEnable()
    {
        script = (PCapsuleArea)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var capsule = script.Capsule;

        SEditorGUI.ChangeCheck(target,
            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Vector3("Center", script.center)
                .OnValueChanged(value => script.center = value)
                + SEditorGUILayout.Float("Radius", script.radius)
                .OnValueChanged(value => script.radius = value)
                + SEditorGUILayout.Float("Height", script.height)
                .OnValueChanged(value => script.height = value)
                + SEditorGUILayout.Int("Direction", script.direction)
                .OnValueChanged(value => script.direction = value)
                + SEditorGUILayout.Color("Color", script.gizmoColor)
                .OnValueChanged(value => script.gizmoColor = value)
                + SEditorGUILayout.Group("World Capsule")
                    .Content(
                        SEditorGUI.DisabledGroup(true)
                        .Content(
                            SEditorGUILayout.Vector3("P1", capsule.point1)
                            + SEditorGUILayout.Vector3("P2", capsule.point2)
                            + SEditorGUILayout.Float("Radius", capsule.radius)
                        )
                    )
            )
        );

        serializedObject.ApplyModifiedProperties();
    }
}
