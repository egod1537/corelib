using System;
using System.Collections.Generic;
using UnityEngine;

namespace Corelib.Utils
{
    [Serializable]
    public class MeshVisualizer : IDisposable, ILifecycleInjectable
    {
        private readonly MonoBehaviour mono;
        private readonly IChunkMeshCreator chunkMeshCreator;
        private Transform meshRoot;

        [Serializable]
        public class ChildMeshDictionary : SerializableDictionary<string, GameObject> { }
        public readonly ChildMeshDictionary childMeshes;

        private Dictionary<string, Texture> runtimeTextureCache;

        private const string MESH_ROOT_NAME = "[Meshes]";

        public MeshVisualizer(MonoBehaviour mono)
        {
            this.mono = mono;
            this.runtimeTextureCache = new();
            this.chunkMeshCreator = new DefaultChunkMeshCreator();
            this.childMeshes = new ChildMeshDictionary();
        }

        public MeshVisualizer(MonoBehaviour mono, IChunkMeshCreator chunkMeshCreator)
        {
            this.mono = mono;
            this.runtimeTextureCache = new();
            this.chunkMeshCreator = chunkMeshCreator;
            this.childMeshes = new ChildMeshDictionary();
        }

        public void OnEnable()
        {
            if (mono == null) return;
            var rootTransform = mono.transform.Find(MESH_ROOT_NAME);
            if (rootTransform == null)
            {
                meshRoot = new GameObject(MESH_ROOT_NAME).transform;
                meshRoot.SetParent(mono.transform);
            }
            else
            {
                meshRoot = rootTransform;
            }

            foreach (Transform child in meshRoot)
            {
                child.gameObject.SetActive(false);
            }
            childMeshes.Clear();
        }

        public void OnDisable()
        {
            if (meshRoot != null)
            {
                UnityEngine.Object.DestroyImmediate(meshRoot.gameObject);
            }
            childMeshes?.Clear();
        }

        public void SetActiveMeshRoot(bool active)
        {
            meshRoot.gameObject.SetActive(active);
        }

        public void ShowMesh(string key) => SetActiveMesh(key, true);

        public void ShowMeshAll()
        {
            if (childMeshes == null) return;
            foreach (string key in childMeshes.Keys)
                SetActiveMesh(key, true);
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
                debugObject = chunkMeshCreator.Create(key);
                debugObject.transform.SetParent(meshRoot);

                var meshFilter = debugObject.GetComponent<MeshFilter>();
                meshRenderer = debugObject.GetComponent<MeshRenderer>();
                meshFilter.mesh = mesh;
                childMeshes[key] = debugObject;

                MeshCollider meshCollider = debugObject.GetComponent<MeshCollider>();
                meshCollider.sharedMesh = null;
                meshCollider.sharedMesh = meshFilter.sharedMesh;
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

        private void SetActiveMesh(string key, bool active)
        {
            if (string.IsNullOrEmpty(key) || childMeshes == null) return;

            if (childMeshes.TryGetValue(key, out var mesh))
            {
                mesh.SetActive(active);
            }
        }

        public void HideMesh(string key) => SetActiveMesh(key, false);

        public void HideMeshAll()
        {
            if (childMeshes == null) return;
            foreach (string key in childMeshes.Keys)
                HideMesh(key);
        }

        public void ClearMeshAll()
        {
            meshRoot.DestroyAllChildrenWithEditor();
            childMeshes?.Clear();
        }

        public void SetMaterial(string key, Material newMaterial)
        {
            if (string.IsNullOrEmpty(key) || childMeshes == null) return;

            if (childMeshes.TryGetValue(key, out var debugObject))
            {
                debugObject.GetComponent<MeshRenderer>().material = newMaterial;
            }
        }

        public void SetMaterial(Material newMaterial)
        {
            if (childMeshes == null) return;
            foreach (string key in childMeshes.Keys)
                SetMaterial(key, newMaterial);
        }

        public void SetPropertyTexture(string key, string property, Texture newTexture)
        {
            if (string.IsNullOrEmpty(key) || childMeshes == null) return;

            if (runtimeTextureCache.TryGetValue(key, out var oldTexture))
            {
                if (oldTexture != null)
                {
#if UNITY_EDITOR
                    if (!Application.isPlaying)
                    {
                        UnityEngine.Object.DestroyImmediate(oldTexture);
                    }
                    else
                    {
                        UnityEngine.Object.Destroy(oldTexture);
                    }
#else
        UnityEngine.Object.Destroy(oldTexture);
#endif
                }
            }
            runtimeTextureCache[key] = newTexture;

            if (childMeshes.TryGetValue(key, out var debugObject))
            {
                var renderer = debugObject.GetComponent<MeshRenderer>();
                if (renderer == null) return;

                var block = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(block);
                block.SetTexture(property, newTexture);
                renderer.SetPropertyBlock(block);
            }
        }

        public void SetPropertyTexture(string property, Texture newTexture)
        {
            if (childMeshes == null) return;
            foreach (string key in childMeshes.Keys)
                SetPropertyTexture(key, property, newTexture);
        }

        public void Dispose()
        {
            foreach (var texture in runtimeTextureCache.Values)
            {
                if (texture == null) continue;

#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    UnityEngine.Object.DestroyImmediate(texture);
                    continue;
                }
#endif
                UnityEngine.Object.Destroy(texture);
            }
            runtimeTextureCache.Clear();
            Debug.Log("RuntimeTextureManager의 모든 텍스처가 해제되었습니다.");
        }
    }
}