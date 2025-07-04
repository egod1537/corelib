using UnityEngine;
using UnityEditor;

namespace Corelib.Utils
{
    public class SOPresetEditor : EditorWindow
    {
        private SOPresetConfig _config;

        [MenuItem("Tools/SO Preset Editor")]
        public static void ShowWindow()
        {
            GetWindow<SOPresetEditor>("SO Presets");
        }

        private void OnGUI()
        {
            GUILayout.Label("SO Preset Config", EditorStyles.boldLabel);

            _config = (SOPresetConfig)EditorGUILayout.ObjectField("Config File", _config, typeof(SOPresetConfig), false);

            EditorGUILayout.Space();

            if (_config == null)
            {
                EditorGUILayout.HelpBox("Please create and assign an SO Preset Config file.", MessageType.Info);
                return;
            }

            GUILayout.Label("Presets", EditorStyles.boldLabel);

            foreach (var preset in _config.presets)
            {
                if (GUILayout.Button(preset.presetName))
                {
                    var asset = Resources.Load(preset.resourcePath);
                    if (asset != null)
                    {
                        Selection.activeObject = asset;
                        EditorGUIUtility.PingObject(asset);
                    }
                    else
                    {
                        Debug.LogError($"[SOPresetEditor] Asset not found at Resources path: '{preset.resourcePath}'");
                    }
                }
            }
        }
    }
}