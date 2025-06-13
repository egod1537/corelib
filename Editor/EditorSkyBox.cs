using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace Corelib.Utils
{
    public static class EditorSkyBox
    {
        private static readonly string PATH = "Editor/Skybox";

        public static Dictionary<string, Material> skybox = new()
        {
            {"Default", Resources.Load<Material>($"{PATH}/Default")},
            {"Prefab", Resources.Load<Material>($"{PATH}/Prefab")}
        };

        public static void Set(string template)
        {
            if (!skybox.ContainsKey(template))
            {
                Debug.LogError("The corresponding SkyBox template does not exist.");
                return;
            }
            RenderSettings.skybox = skybox[template];
        }
    }
}