using UnityEngine;
using System.Collections.Generic;

namespace Corelib.Utils
{
    [System.Serializable]
    public class SOPreset
    {
        public string presetName;
        public string resourcePath;
    }

    [CreateAssetMenu(fileName = "SOPresetConfig", menuName = "Editor/SO Preset Config")]
    public class SOPresetConfig : ScriptableObject
    {
        public List<SOPreset> presets = new List<SOPreset>();
    }
}
