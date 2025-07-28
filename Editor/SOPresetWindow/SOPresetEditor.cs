using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System.Linq;

namespace Corelib.Utils
{
    public class SOPresetEditor : EditorWindow
    {
        [SerializeField] private SOPresetConfig _config;
        [SerializeField] private UnityEngine.Object _selectedAsset;
        private Editor _assetEditor;
        [SerializeField] private Vector2 _scrollPos;

        private SearchField _searchField;
        [SerializeField] private string _searchText = string.Empty;
        [SerializeField] private int _selectedPresetIndex = -1;

        [MenuItem("Tools/SO Preset Editor")]
        public static void ShowWindow()
        {
            GetWindow<SOPresetEditor>("SO Presets");
        }

        private void OnEnable()
        {
            _searchField = new SearchField();
            titleContent = EditorGUIUtility.TrTextContent(
                "SO Presets",
                EditorGUIUtility.IconContent("ScriptableObject Icon").image);
            if (_selectedAsset != null && _assetEditor == null)
            {
                _assetEditor = Editor.CreateEditor(_selectedAsset);
            }
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

            if (_searchField != null)
            {
                _searchText = _searchField.OnGUI(_searchText);
            }
            else
            {
                _searchText = EditorGUILayout.TextField(_searchText);
            }

            var filteredPresets = _config.presets
                .Where(p => string.IsNullOrEmpty(_searchText) || p.presetName.IndexOf(_searchText, System.StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            var presetNames = filteredPresets.Select(p => p.presetName).ToArray();

            int newIndex = _selectedPresetIndex;
            if (presetNames.Length > 0)
            {
                EditorGUILayout.BeginHorizontal();
                newIndex = EditorGUILayout.Popup("Select Preset", Mathf.Clamp(_selectedPresetIndex, 0, presetNames.Length - 1), presetNames);
                if (GUILayout.Button("Select", GUILayout.Width(60)))
                {
                    if (_selectedPresetIndex >= 0 && _selectedPresetIndex < filteredPresets.Count)
                    {
                        var preset = filteredPresets[_selectedPresetIndex];
                        var asset = Resources.Load(preset.resourcePath);
                        if (asset != null)
                        {
                            EditorUtility.FocusProjectWindow();
                            Selection.activeObject = asset;
                            EditorGUIUtility.PingObject(asset);
                        }
                        else
                        {
                            Debug.LogError($"[SOPresetEditor] Asset not found at Resources path: '{preset.resourcePath}'");
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.Label("No presets found.");
                newIndex = -1;
            }

            if (newIndex != _selectedPresetIndex)
            {
                _selectedPresetIndex = newIndex;
                if (_selectedPresetIndex >= 0)
                {
                    SelectPreset(filteredPresets[_selectedPresetIndex]);
                }
            }

            if (_selectedAsset != null && _assetEditor != null)
            {
                EditorGUILayout.Space();
                GUILayout.Label($"Editing: {_selectedAsset.name}", EditorStyles.boldLabel);
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
                _assetEditor.OnInspectorGUI();
                EditorGUILayout.EndScrollView();
            }
        }

        private void SelectPreset(SOPreset preset)
        {
            var asset = Resources.Load(preset.resourcePath);
            if (asset != null)
            {
                _selectedAsset = asset;
                if (_assetEditor != null)
                {
                    DestroyImmediate(_assetEditor);
                }
                _assetEditor = Editor.CreateEditor(_selectedAsset);
            }
            else
            {
                Debug.LogError($"[SOPresetEditor] Asset not found at Resources path: '{preset.resourcePath}'");
            }
        }

        private void OnDisable()
        {
            if (_assetEditor != null)
            {
                DestroyImmediate(_assetEditor);
            }
        }
    }
}
