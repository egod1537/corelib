using System;
using System.Collections.Generic;
using UnityEngine;

namespace Corelib.Utils
{
    [Serializable]
    public class MeshVisualizer : ILifecycleInjectable
    {
        private readonly MonoBehaviour mono;
        private Transform debugRoot;

        [Serializable]
        public class ChildMeshDictionary : SerializableDictionary<string, GameObject> { }
        public readonly ChildMeshDictionary childMeshes;

        private const string MESH_ROOT_NAME = "[Meshes]";

        public MeshVisualizer(MonoBehaviour mono)
        {
            this.mono = mono;
            this.childMeshes = new ChildMeshDictionary();
        }

        public void OnEnable()
        {
            if (mono == null) return;
            var rootTransform = mono.transform.Find(MESH_ROOT_NAME);
            if (rootTransform == null)
            {
                debugRoot = new GameObject(MESH_ROOT_NAME).transform;
                debugRoot.SetParent(mono.transform);
            }
            else
            {
                debugRoot = rootTransform;
            }

            foreach (Transform child in debugRoot)
            {
                child.gameObject.SetActive(false);
            }
            childMeshes.Clear();
        }

        public void OnDisable()
        {
            if (debugRoot != null)
            {
                UnityEngine.Object.DestroyImmediate(debugRoot.gameObject);
            }
            childMeshes?.Clear();
        }

        public GameObject ShowMesh(string key, Mesh mesh, Material overrideMaterial = null)
        {
            if (string.IsNullOrEmpty(key) || mesh == null) return null;

            GameObject debugObject;
            MeshRenderer meshRenderer;

            if (childMeshes.TryGetValue(key, out debugObject))
            {
                debugObject.SetActive(true);
                var meshFilter = debugObject.GetComponent<MeshFilter>();
                meshRenderer = debugObject.GetComponent<MeshRenderer>();
                if (meshFilter != null)
                {
                    meshFilter.mesh = mesh;
                }
            }
            else
            {
                debugObject = new GameObject(key);
                debugObject.transform.SetParent(debugRoot);

                var meshFilter = debugObject.AddComponent<MeshFilter>();
                meshRenderer = debugObject.AddComponent<MeshRenderer>();
                meshFilter.mesh = mesh;
                childMeshes[key] = debugObject;
            }

            if (meshRenderer != null)
            {
                if (overrideMaterial != null)
                {
                    meshRenderer.material = overrideMaterial;
                }
                else
                {
                    int subMeshCount = mesh.subMeshCount;
                    var materials = new Material[subMeshCount];
                    Color[] debugColors = { new Color(0, 1, 0, 0.5f), new Color(1, 1, 0, 0.5f), Color.blue, Color.red };

                    for (int i = 0; i < subMeshCount; i++)
                    {
                        var mat = new Material(Shader.Find("Unlit/Color"));
                        mat.color = debugColors[i % debugColors.Length];
                        materials[i] = mat;
                    }
                    meshRenderer.materials = materials;
                }
            }

            return debugObject;
        }

        public void HideMesh(string key)
        {
            if (string.IsNullOrEmpty(key)) return;

            if (childMeshes.TryGetValue(key, out var debugObject))
            {
                debugObject.SetActive(false);
            }
        }

        public void HideMeshAll()
        {
            foreach (string key in childMeshes.Keys)
                HideMesh(key);
        }

        private Color GetColorForKey(string key)
        {
            int hash = key.GetHashCode();
            float r = (hash & 0xFF) / 255f;
            float g = ((hash >> 8) & 0xFF) / 255f;
            float b = ((hash >> 16) & 0xFF) / 255f;
            return new Color(r, g, b, 0.5f);
        }
    }
}