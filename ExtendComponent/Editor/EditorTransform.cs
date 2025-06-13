using UnityEngine;
using UnityEditor;

namespace Corelib.Utils
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Transform))]
    public partial class EditorTransform : InnerEditor<Transform>
    {
        public override void OnEnable()
        {
            base.OnEnable();
            // AddInnerGUI<ComponentCategoryGUI>();

            // script.gameObject.
            // MaybeAddComponent<ComponentCategory>().hideFlags = HideFlags.HideInInspector;

            base.OnEnableInnerGUI();
        }

        public override void OnInspectorGUI()
        {
            DrawLocal();
            DrawWorld();

            base.OnInspectorInnerGUI();
        }

        private void DrawLocal()
        {
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.BeginVertical("HelpBox");
                {
                    EditorGUILayout.LabelField("[Local]", new GUIStyle().Label().SetBold(true));
                }
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel++;
                Undo.RecordObject(script.transform, $"Local {script.gameObject.name}");
                script.localPosition = CustomVector3Field("Position", script.localPosition, Vector3.zero);
                script.localEulerAngles = CustomVector3Field("Rotation", script.localEulerAngles, Vector3.zero);
                script.localScale = CustomVector3Field("Scale", script.localScale, Vector3.one);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        private Vector3 CustomVector3Field(string title, Vector3 vec3, Vector3 initialVector)
        {
            Vector3 ret = vec3;
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(title[0].ToString(), GUILayout.Width(32)))
                {
                    return initialVector;
                }
                EditorGUILayout.LabelField(title, GUILayout.Width(64));
                ret = EditorGUILayout.Vector3Field("", vec3);
            }
            EditorGUILayout.EndHorizontal();
            return ret;
        }

        bool isFoldWorld;

        private void DrawWorld()
        {
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical("HelpBox");
                {
                    isFoldWorld = EditorGUILayout.Foldout(isFoldWorld, "[World (Read Only)]",
                        new GUIStyle().FoldOut().SetBold(true));
                }
                EditorGUILayout.EndVertical();
                if (isFoldWorld)
                {
                    EditorGUILayout.Vector3Field("Position", script.position);
                    EditorGUILayout.Vector3Field("Rotation", script.eulerAngles);
                    EditorGUILayout.Vector3Field("Scale", script.lossyScale);
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }
    }
}